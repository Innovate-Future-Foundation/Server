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

                            # Build images
                            docker-compose build --no-cache base
                            docker-compose build --no-cache api

                            # Start postgres and wait for it to be ready
                            docker-compose up -d postgres

                            # Wait for postgres to be fully initialized
                            echo "Waiting for postgres to be ready..."
                            RETRIES=30
                            until docker-compose exec -T postgres pg_isready -h localhost -U ${DB_USER} -d ${DB_NAME} || [ $RETRIES -eq 0 ]; do
                                echo "Waiting for postgres server, $((RETRIES--)) remaining attempts..."
                                sleep 2
                            done

                            if [ $RETRIES -eq 0 ]; then
                                echo "Failed to connect to postgres"
                                exit 1
                            fi

                            # Run migrations
                            echo "Running database migrations..."
                            docker-compose up -d migration

                            # Wait for migration to complete
                            sleep 10

                            # Check migration logs for success
                            if docker-compose logs migration | grep -q "Build failed"; then
                                echo "Migration failed"
                                exit 1
                            fi

                            # Start remaining services
                            docker-compose up -d api pgadmin

                            # Verify all services
                            echo "Verifying services..."
                            docker-compose ps

                            # Wait for API to be ready
                            echo "Waiting for API to be ready..."
                            sleep 10
                        '''
                    } catch (Exception e) {
                        sh '''
                            echo "Deployment failed. Collecting logs..."
                            docker-compose logs
                        '''
                        throw e
                    }
                }
            }
        }
    }

    post {
        failure {
            script {
                sh '''
                    echo "Pipeline failed. Collecting logs..."
                    docker-compose logs || true
                '''
            }
        }
        always {
            script {
                sh '''
                    if [ -f "docker-compose.yml" ]; then
                        echo "Cleaning up resources..."
                        docker-compose down --remove-orphans -v || true
                        docker images -q -f "dangling=true" | xargs -r docker rmi || true
                    fi
                '''
            }
        }
    }
}