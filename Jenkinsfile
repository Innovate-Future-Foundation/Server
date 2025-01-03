pipeline {
    agent any

    environment {
        // Define global environment variables
        DOCKER_BUILDKIT = '1'
        COMPOSE_PROJECT_NAME = "${env.JOB_NAME}-${env.BUILD_ID}"
        DOCKER_CACHE_TTL = '24h'
        HEALTH_CHECK_RETRIES = '5'
        HEALTH_CHECK_INTERVAL = '5'
        DB_INIT_TIMEOUT = '30'
    }

    options {
        // Pipeline specific options
        timestamps()  // Add timestamps to console output
        timeout(time: 30, unit: 'MINUTES')  // Set timeout
        disableConcurrentBuilds()  // Prevent parallel execution
        ansiColor('xterm')  // Colored output
    }

    stages {
        stage('Checkout') {
            steps {
                cleanWs()  // Clean workspace before checkout
                checkout scm
            }
        }

        stage('Pre-build Cleanup') {
            steps {
                script {
                    sh '''
                        # Cleanup running containers and resources
                        if [ -f "docker-compose.yml" ]; then
                            docker-compose down --remove-orphans -v || true
                            docker system prune -f --filter "until=24h"
                            docker volume prune -f
                            docker network prune -f
                        fi
                    '''
                }
            }
        }

        stage('Environment Setup') {
            steps {
                script {
                    sh '''
                        # Validate environment file exists
                        if [ ! -f ".env.example" ]; then
                            echo "ERROR: .env.example file not found"
                            exit 1
                        fi

                        # Setup environment file
                        cp .env.example .env
                    '''
                }
            }
        }

        stage('Build and Test') {
            steps {
                script {
                    try {
                        sh '''
                            # Build with cache optimization using global env
                            docker-compose build --parallel base api

                            # Use project name in verification
                            if ! docker images | grep -q "${COMPOSE_PROJECT_NAME}"; then
                                echo "ERROR: Build failed - image not found"
                                exit 1
                            fi
                        '''
                    } catch (Exception e) {
                        error "Build failed: ${e.message}"
                    }
                }
            }
        }

        stage('Database Setup') {
            steps {
                script {
                    try {
                        sh '''
                            docker-compose up -d postgres

                            echo "Waiting for database to be ready..."
                            for i in $(seq 1 ${DB_INIT_TIMEOUT}); do
                                if docker-compose exec -T postgres pg_isready -h localhost; then
                                    echo "Database is ready"
                                    break
                                fi
                                echo "Attempt $i/${DB_INIT_TIMEOUT}: Database not ready yet..."
                                sleep 2
                                if [ $i -eq ${DB_INIT_TIMEOUT} ]; then
                                    echo "ERROR: Database failed to start"
                                    exit 1
                                fi
                            done
                        '''
                    } catch (Exception e) {
                        error "Database setup failed: ${e.message}"
                    }
                }
            }
        }

        stage('Migration') {
            steps {
                script {
                    try {
                        sh '''
                            echo "Running database migrations..."
                            docker-compose up -d migration

                            # Monitor migration logs
                            sleep 10
                            if docker-compose logs migration | grep -E "error|Error|ERROR|failed|Failed|FAILED"; then
                                echo "ERROR: Migration failed"
                                docker-compose logs migration
                                exit 1
                            fi
                        '''
                    } catch (Exception e) {
                        error "Migration failed: ${e.message}"
                    }
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    try {
                        sh '''
                            docker-compose up -d api pgadmin

                            echo "Verifying deployment..."
                            sleep 10

                            for i in $(seq 1 ${HEALTH_CHECK_RETRIES}); do
                                if curl -sf http://localhost:5091/health; then
                                    echo "API is healthy"
                                    break
                                fi
                                echo "Attempt $i/${HEALTH_CHECK_RETRIES}: API not healthy yet..."
                                sleep ${HEALTH_CHECK_INTERVAL}
                                if [ $i -eq ${HEALTH_CHECK_RETRIES} ]; then
                                    echo "ERROR: API health check failed"
                                    exit 1
                                fi
                            done
                        '''
                    } catch (Exception e) {
                        error "Deployment failed: ${e.message}"
                    }
                }
            }
        }
    }

    post {
        success {
            script {
                echo "Pipeline completed successfully"
            }
        }
        failure {
            script {
                sh '''
                    echo "Collecting failure logs..."
                    docker-compose logs || true
                '''
            }
        }
        always {
            script {
                sh '''
                    echo "Cleaning up resources..."
                    docker-compose down --remove-orphans -v || true
                    docker system prune -f --filter "until=${DOCKER_CACHE_TTL}"
                '''
            }
        }
    }
}