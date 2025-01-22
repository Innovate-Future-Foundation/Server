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
                    sh 'docker compose build'
                }
            }
        }

stage('Push Docker Images') {
    steps {
        withAWS(credentials: 'aws-credentials', region: AWS_REGION) {
            script {
                // Authenticate with ECR
                sh "aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${ECR_REPO}"
                
                // Tag and push inff-api-build
                sh "docker tag inff-api-build:latest ${ECR_REPO}:inff-api-build"
                sh "docker push ${ECR_REPO}:inff-api-build"
                
                // Tag and push server-api
                sh "docker tag server-api:latest ${ECR_REPO}:server-api"
                sh "docker push ${ECR_REPO}:server-api"
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
