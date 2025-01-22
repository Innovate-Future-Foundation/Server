pipeline {
    agent any

    environment {
        AWS_REGION = 'ap-southeast-2'
        ECR_REPO = '986643365562.dkr.ecr.ap-southeast-2.amazonaws.com/ecr-repo'
        IMAGE_NAME = 'inff-api-build'
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
                withAWS(credentials: 'aws-credentials', region: '${AWS_REGION}') {
                    script {
                        sh "aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${ECR_REPO}"
                        
                        def buildNumber = env.BUILD_NUMBER
                        sh "docker tag ${IMAGE_NAME}:latest ${ECR_REPO}:${buildNumber}"
                        sh "docker push ${ECR_REPO}:${buildNumber}"
                        
                        sh "docker tag ${IMAGE_NAME}:latest ${ECR_REPO}:api-${buildNumber}"
                        sh "docker push ${ECR_REPO}:api-${buildNumber}"
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
