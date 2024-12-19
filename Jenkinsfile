pipeline {
    agent any

    environment {
        DOTNET_SDK_VERSION = '8.0'
        PROJECT_NAME = 'InnovateFuture'
        DOCKER_IMAGE_NAME = 'innovatefuture-api'
        AWS_REGION = 'us-east-1'
        AWS_ACCOUNT_ID = credentials('aws-account-id')
        DOCKER_REGISTRY = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com"
        DOCKER_CREDENTIALS_ID = 'ecr-credentials'
        DB_CONNECTION_STRING = credentials('db-connection-string')
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh '''
                    dotnet test tests/InnovateFuture.Api.Tests/InnovateFuture.Api.Tests.csproj --no-restore
                    dotnet test tests/InnovateFuture.Application.Tests/InnovateFuture.Application.Tests.csproj --no-restore
                    dotnet test tests/InnovateFuture.Domain.Tests/InnovateFuture.Domain.Tests.csproj --no-restore
                '''
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish src/InnovateFuture.Api/InnovateFuture.Api.csproj -c Release -o publish'
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    docker.build("${DOCKER_IMAGE_NAME}:${BUILD_NUMBER}")
                }
            }
        }

        stage('Docker Login') {
            steps {
                script {
                    sh """
                        aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${DOCKER_REGISTRY}
                    """
                }
            }
        }

        stage('Push Docker Image') {
            steps {
                script {
                    docker.withRegistry("https://${DOCKER_REGISTRY}") {
                        docker.image("${DOCKER_IMAGE_NAME}:${BUILD_NUMBER}").push()
                        docker.image("${DOCKER_IMAGE_NAME}:${BUILD_NUMBER}").push('latest')
                    }
                }
            }
        }

        stage('Database Migration') {
            steps {
                script {
                    sh '''
                        cd src/InnovateFuture.Api
                        dotnet tool install --global dotnet-ef --version 8.0.0
                        export PATH="$PATH:$HOME/.dotnet/tools"
                        dotnet ef database update --connection "${DB_CONNECTION_STRING}"
                    '''
                }
            }
        }

        stage('Deploy') {
            steps {
                // Add your deployment steps here
                // Example: Deploy to EC2 using SSH or AWS CLI
                sh '''
                    echo "Deploying to EC2..."
                    # Add your deployment commands
                '''
            }
        }
    }

    post {
        always {
            node {
                cleanWs()
            }
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
}