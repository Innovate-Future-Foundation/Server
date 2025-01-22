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

        stage('Login to ECR') {
            steps {
                script {
                    sh "aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${ECR_REPO}"
                }
            }
        }

        stage('Push Docker Images') {
            steps {
                script {
                    def buildNumber = env.BUILD_NUMBER
                    sh "docker tag ${IMAGE_NAME}:latest ${ECR_REPO}:${buildNumber}"
                    sh "docker push ${ECR_REPO}:${buildNumber}"
                    
                    // Tag and push the API image
                    sh "docker tag inff-api-build:latest ${ECR_REPO}:api-${buildNumber}"
                    sh "docker push ${ECR_REPO}:api-${buildNumber}"
                }
            }
        }

        stage('Clean Up') {
            steps {
                script {
                    sh 'docker-compose -f ${DOCKER_COMPOSE_FILE} down --rmi all --volumes --remove-orphans'
                    sh "docker rmi ${ECR_REPO}:${BUILD_NUMBER}"
                    sh "docker rmi ${ECR_REPO}:api-${BUILD_NUMBER}"
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}
