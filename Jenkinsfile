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
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        DOTNET_NOLOGO = 'true'
        DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
    }

    options {
        skipDefaultCheckout()
        timestamps()
        parallelsAlwaysFailFast()
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build and Test') {
            parallel {
                stage('Build') {
                    steps {
                        sh '''
                            dotnet restore --force-evaluate
                            dotnet build --configuration Release --no-restore
                        '''
                    }
                }

                stage('Test') {
                    steps {
                        sh '''
                            dotnet test tests/InnovateFuture.Api.Tests/InnovateFuture.Api.Tests.csproj --configuration Release --no-build
                            dotnet test tests/InnovateFuture.Application.Tests/InnovateFuture.Application.Tests.csproj --configuration Release --no-build
                            dotnet test tests/InnovateFuture.Domain.Tests/InnovateFuture.Domain.Tests.csproj --configuration Release --no-build
                        '''
                    }
                }
            }
        }

        stage('Publish and Docker') {
            parallel {
                stage('Publish') {
                    steps {
                        sh 'dotnet publish src/InnovateFuture.Api/InnovateFuture.Api.csproj -c Release -o publish --no-restore --no-build'
                    }
                }

                stage('Docker Build') {
                    steps {
                        script {
                            docker.build("${DOCKER_IMAGE_NAME}:${BUILD_NUMBER}", '--no-cache .')
                        }
                    }
                }
            }
        }

        stage('Docker Push') {
            steps {
                script {
                    sh """
                        aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${DOCKER_REGISTRY}
                    """
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
                        dotnet tool install --global dotnet-ef --version 8.0.0 --no-cache
                        export PATH="$PATH:$HOME/.dotnet/tools"
                        dotnet ef database update --connection "${DB_CONNECTION_STRING}"
                    '''
                }
            }
        }

        stage('Deploy') {
            steps {
                sh '''
                    echo "Deploying to EC2..."
                    # Add your deployment commands
                '''
            }
        }
    }

    post {
        always {
            cleanWs()
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
}