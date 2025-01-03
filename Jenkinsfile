pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Clean') {
            steps {
                script {
                    // Clean workspace but preserve checked out files
                    sh '''
                        if [ -f "docker-compose.yml" ]; then
                            docker-compose down --remove-orphans -v || true
                            docker system prune -f
                        fi
                    '''
                }
            }
        }

        stage('Setup Environment') {
            steps {
                script {
                    sh '''
                        if [ -f ".env.example" ]; then
                            cp .env.example .env
                        else
                            echo ".env.example file not found"
                            exit 1
                        fi
                    '''
                }
            }
        }

        stage('Build and Deploy') {
            steps {
                script {
                    try {
                        // Verify docker-compose file exists
                        sh '''
                            if [ ! -f "docker-compose.yml" ]; then
                                echo "docker-compose.yml not found"
                                exit 1
                            fi
                        '''

                        // Build and start all services
                        sh '''
                            docker-compose build --no-cache base
                            docker-compose build --no-cache api
                            docker-compose up -d postgres
                            sleep 15

                            # Check if postgres is ready
                            docker-compose exec -T postgres pg_isready -h localhost -p 5432

                            if [ $? -eq 0 ]; then
                                docker-compose up -d migration
                                docker-compose up -d api pgadmin
                            else
                                echo "Postgres is not ready. Aborting."
                                exit 1
                            fi
                        '''

                        // Verify containers
                        sh '''
                            docker-compose ps
                            docker-compose logs migration
                        '''
                    } catch (Exception e) {
                        sh 'docker-compose logs || true'
                        throw e
                    }
                }
            }
        }
    }

    post {
        failure {
            script {
                sh 'docker-compose logs || true'
            }
        }
        always {
            script {
                sh '''
                    if [ -f "docker-compose.yml" ]; then
                        docker-compose down --remove-orphans -v || true
                        docker images -q -f "dangling=true" | xargs -r docker rmi || true
                    fi
                '''
            }
        }
    }
}