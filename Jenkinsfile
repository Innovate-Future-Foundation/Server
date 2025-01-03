pipeline {
    agent any

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

        stage('Setup Environment') {
            steps {
                script {
                    sh 'cp .env.example .env'
                }
            }
        }

        stage('Build and Deploy') {
            steps {
                script {
                    // Stop any existing containers and remove them
                    sh 'docker-compose down --remove-orphans'

                    // Build and start all services defined in docker-compose.yml
                    sh '''
                        docker-compose build --no-cache base
                        docker-compose build --no-cache api
                        docker-compose up -d postgres
                        sleep 10  # Give postgres time to initialize
                        docker-compose up -d migration
                        docker-compose up -d api pgadmin
                    '''

                    // Verify containers are running
                    sh 'docker-compose ps'
                }
            }
        }
    }

    post {
        failure {
            script {
                sh 'docker-compose logs'  // Print logs if something fails
            }
        }
        always {
            script {
                // Clean up dangling images
                sh '''
                    docker images -q -f "dangling=true" | xargs -r docker rmi || true
                '''
            }
        }
    }
}