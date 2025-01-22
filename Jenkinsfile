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
                    sh 'docker-compose -f ${DOCKER_COMPOSE_FILE} build --no-cache'
                }
            }
        }

        stage('Push Docker Images') {
            steps {
                withAWS(credentials: 'aws-credentials', region: '${AWS_REGION}') {
                    script {
                        def buildNumber = env.BUILD_NUMBER
                        def imageTags = ["${buildNumber}", "api-${buildNumber}", "latest"]
                        
                        sh "aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${ECR_REPO}"
                        
                        imageTags.each { tag ->
                            sh "docker tag ${IMAGE_NAME}:latest ${ECR_REPO}:${tag}"
                            sh "docker push ${ECR_REPO}:${tag}"
                        }
                    }
                }
            }
        }

        stage('Clean Up') {
            steps {
                script {
                    sh 'docker-compose -f ${DOCKER_COMPOSE_FILE} down --rmi all --volumes --remove-orphans'
                    sh "docker system prune -af"
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
        success {
            echo 'Pipeline executed successfully!'
        }
        failure {
            echo 'Pipeline execution failed. Please check the logs for details.'
        }
    }
}
