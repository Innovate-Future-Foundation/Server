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
                    // Kill any process using port 5091
                    sh '''
                        if lsof -Pi :5091 -sTCP:LISTEN -t >/dev/null ; then
                            lsof -Pi :5091 -sTCP:LISTEN -t | xargs kill -9 || true
                        fi

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
                        sh '''
                            if [ ! -f "docker-compose.yml" ]; then
                                echo "docker-compose.yml not found"
                                exit 1
                            fi
                        '''

                        // Build and start services in the correct order
                        sh '''
                            # Build base and api images
                            docker-compose build --no-cache base
                            docker-compose build --no-cache api

                            # Start postgres first
                            docker-compose up -d postgres
                            sleep 15

                            # Verify postgres is ready
                            until docker-compose exec -T postgres pg_isready -h localhost -p 5432; do
                                echo "Waiting for postgres..."
                                sleep 5
                            done

                            # Run migrations
                            docker-compose up -d migration

                            # Wait for migration to complete
                            docker-compose logs -f migration &
                            MIGRATION_PID=$!
                            sleep 10
                            kill $MIGRATION_PID || true

                            # Start API and pgAdmin
                            docker-compose up -d api pgadmin

                            # Verify all services are running
                            docker-compose ps
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