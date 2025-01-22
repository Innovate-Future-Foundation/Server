pipeline {
    agent any

    environment {
        AWS_REGION = 'ap-southeast-2'
        ECR_REPO = '986643365562.dkr.ecr.ap-southeast-2.amazonaws.com/ecr-repo'
        BASE_IMAGE_NAME = 'inff-api-build'
        API_IMAGE_NAME = 'server-api'
        DOCKER_COMPOSE_FILE = 'docker-compose.yml'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Docker Images') {
            steps {
                script {
                    sh 'docker-compose -f ${DOCKER_COMPOSE_FILE} build'
                }
            }
        }

        stage('Push Docker Images') {
            steps {
                withAWS(credentials: 'aws-credentials', region: AWS_REGION) {
                    script {
                        echo "Pushing Docker images to region: ${AWS_REGION}"
                        sh "aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${ECR_REPO}"
                        
                        def buildNumber = env.BUILD_NUMBER
                        
                        // Tag and push base image (inff-api-build)
                        sh "docker tag ${BASE_IMAGE_NAME}:latest ${ECR_REPO}:${BASE_IMAGE_NAME}-${buildNumber}"
                        sh "docker push ${ECR_REPO}:${BASE_IMAGE_NAME}-${buildNumber}"
                        
                        // Tag and push API image (server-api)
                        sh "docker tag ${API_IMAGE_NAME}:latest ${ECR_REPO}:${API_IMAGE_NAME}-${buildNumber}"
                        sh "docker push ${ECR_REPO}:${API_IMAGE_NAME}-${buildNumber}"
                    }
                }
            }
        }

        stage('Clean Up') {
            steps {
                script {
                    sh 'docker-compose -f ${DOCKER_COMPOSE_FILE} down --rmi all --volumes --remove-orphans'
                    
                    // Use try-catch to handle potential errors during image removal
                    try {
                        sh 'docker rmi $(docker images -f "dangling=true" -q) || true'
                    } catch (Exception e) {
                        echo "Error during image removal: ${e.getMessage()}"
                    }
                }
            }
        }
    }
}
