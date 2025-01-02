pipeline {
    agent any

    environment {
        APP_NAME = 'inff-api'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Base Image') {
            steps {
                script {
                    sh 'docker build -t inff-api-build -f Dockerfile.base .'
                }
            }
        }

        stage('Run Tests') {
            steps {
                script {
                    sh '''
                        docker build -t ${APP_NAME}-test -f Dockerfile.test .
                        docker run --rm ${APP_NAME}-test
                    '''
                }
            }
        }

        stage('Build API Image') {
            steps {
                script {
                    sh 'docker build -t ${APP_NAME}:${BUILD_NUMBER} -f Dockerfile.api .'
                }
            }
        }
    }

    post {
        always {
            cleanWs()
            sh '''
                docker rmi ${APP_NAME}-test || true
                docker rmi ${APP_NAME}:${BUILD_NUMBER} || true
            '''
        }
    }
}