pipeline {
    agent any

    stages {
        stage('Clean') {
            steps {
                cleanWs()
                script {
                    // Clean up any existing containers and ports
                    sh '''
                        docker-compose down --remove-orphans -v
                        docker system prune -f
                    '''
                }
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
                    try {
                        // Build and start all services defined in docker-compose.yml
                        sh '''
                            docker-compose build --no-cache base
                            docker-compose build --no-cache api
                            docker-compose up -d postgres
                            sleep 15  # Increased wait time for postgres

                            # Check if postgres is ready
                            docker-compose exec -T postgres pg_isready -h localhost -p 5432

                            # Only proceed with migration if postgres is ready
                            if [ $? -eq 0 ]; then
                                docker-compose up -d migration
                                docker-compose up -d api pgadmin
                            else
                                echo "Postgres is not ready. Aborting."
                                exit 1
                            fi
                        '''

                        // Verify containers are running
                        sh '''
                            docker-compose ps
                            docker-compose logs migration  # Check migration logs
                        '''
                    } catch (Exception e) {
                        sh 'docker-compose logs'
                        throw e
                    }
                }
            }
        }
    }

    post {
        failure {
            script {
                sh 'docker-compose logs'
            }
        }
        always {
            script {
                // Clean up
                sh '''
                    docker-compose down --remove-orphans -v
                    docker images -q -f "dangling=true" | xargs -r docker rmi || true
                '''
            }
        }
    }
}