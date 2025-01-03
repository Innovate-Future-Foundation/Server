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
                            # Build images
                            docker-compose build --no-cache base
                            docker-compose build --no-cache api

                            # Start postgres
                            docker-compose up -d postgres

                            # Wait for postgres to be ready
                            echo "Waiting for postgres to be ready..."
                            for i in $(seq 1 30); do
                                if docker-compose exec -T postgres pg_isready; then
                                    echo "Postgres is ready!"
                                    break
                                fi
                                echo "Waiting for postgres... $i/30"
                                sleep 2
                                if [ $i -eq 30 ]; then
                                    echo "Timeout waiting for postgres"
                                    exit 1
                                fi
                            done

                            # Run migrations
                            echo "Running database migrations..."
                            docker-compose up -d migration

                            # Wait for migration to complete and check logs
                            sleep 10
                            if docker-compose logs migration | grep -q "error\|Error\|ERROR"; then
                                echo "Migration failed"
                                docker-compose logs migration
                                exit 1
                            fi

                            # Start remaining services
                            echo "Starting API and pgAdmin..."
                            docker-compose up -d api pgadmin

                            # Final verification
                            echo "Verifying services..."
                            docker-compose ps

                            # Wait for services to be ready
                            sleep 10

                            echo "Deployment completed successfully"
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