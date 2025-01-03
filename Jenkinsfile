pipeline {
    agent any

    environment {
        APP_NAME = 'inff-api-build'
        CONTAINER_NAME = 'inff-api-container'
    }

    stages {
        stage('Clean') {
            steps {
                cleanWs()
            }
        }

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Base Image') {
            steps {
                script {
                    sh '''
                        echo "Starting base image build..."
                        docker build --progress=plain -t inff-api-build:latest -f Dockerfile.base . 2>&1 | tee build.log
                        echo "Build completed. Log saved to build.log"
                        cat build.log
                    '''
                }
            }
        }

        stage('Build and Deploy API') {
            steps {
                script {
                    // Stop and remove existing container if it exists
                    sh '''
                        docker stop ${CONTAINER_NAME} || true
                        docker rm ${CONTAINER_NAME} || true
                    '''

                    // Build new image
                    sh 'docker build -t ${APP_NAME}:${BUILD_NUMBER} -f Dockerfile.api .'

                    sh 'mv .env.example .env'

                    // Start new container
                    sh '''
                        docker run -d \
                            --name ${CONTAINER_NAME} \
                            -p 5091:5091 \
                            --env-file .env \
                            ${APP_NAME}:${BUILD_NUMBER}
                    '''
                }
            }
        }
    }

    post {
        always {
            sh '''
                docker images -q -f "dangling=true" | xargs -r docker rmi
            '''
        }
    }
}