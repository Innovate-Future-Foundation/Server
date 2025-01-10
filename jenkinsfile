pipeline {
    agent any

    environment {
        // Core Docker configuration
        DOCKER_BUILDKIT = '1'  // Enable BuildKit for better performance
        COMPOSE_PROJECT_NAME = "${env.JOB_NAME}-${env.BUILD_ID}"  // Unique project name per build
        
        // Timeouts and retry configurations
        DOCKER_CACHE_TTL = '24h'
        HEALTH_CHECK_RETRIES = '5'
        HEALTH_CHECK_INTERVAL = '5'
        DB_INIT_TIMEOUT = '30'
        
        // Service endpoints configuration
        EC2_HOST = credentials('EC2_HOST')  // Store sensitive data in Jenkins credentials
        API_URL = "http://${EC2_HOST}:5091"
        PGADMIN_URL = "http://${EC2_HOST}:5050"
        
        // Add monitoring configurations
        PROMETHEUS_PORT = '9090'
        GRAFANA_PORT = '3000'
    }

    options {
        timestamps()  // Add timestamps to console output
        timeout(time: 30, unit: 'MINUTES')  // Set global timeout
        disableConcurrentBuilds()  // Prevent parallel execution
        ansiColor('xterm')  // Enable colored output
    }

    stages {
        stage('Checkout') {
            steps {
                cleanWs()  // Clean workspace before checkout
                checkout scm
                
                // Log git information for traceability
                sh '''
                    git log -1
                    git status
                '''
            }
        }

        stage('Pre-build Cleanup') {
            steps {
                script {
                    sh '''
                        # Enhanced cleanup with error handling
                        echo "Starting cleanup process..."
                        
                        # Cleanup specific containers with safety checks
                        for container in container-postgres container-pgadmin; do
                            if docker ps -a | grep -q $container; then
                                docker rm -f $container || echo "Failed to remove $container"
                            fi
                        done

                        # Cleanup docker resources with age filter
                        docker system prune -f --filter "until=${DOCKER_CACHE_TTL}"
                        docker volume prune -f --filter "label!=keep"
                        docker network prune -f --filter "until=${DOCKER_CACHE_TTL}"
                    '''
                }
            }
        }

        stage('Environment Setup') {
            steps {
                script {
                    sh '''
                        # Enhanced environment validation
                        for file in .env.example docker-compose.yml; do
                            if [ ! -f "$file" ]; then
                                echo "ERROR: Required file $file not found"
                                exit 1
                            fi
                        done

                        # Create environment file with additional checks
                        cp .env.example .env
                        chmod 600 .env  # Secure file permissions
                        
                        # Validate environment variables
                        if ! grep -q "DB_NAME" .env; then
                            echo "ERROR: Missing required environment variables"
                            exit 1
                        fi
                    '''
                }
            }
        }

        stage('Build and Test') {
            steps {
                script {
                    try {
                        sh '''
                            # Build with enhanced caching and parallel processing
                            DOCKER_BUILDKIT=1 docker compose build \
                                --parallel \
                                --build-arg BUILDKIT_INLINE_CACHE=1 \
                                --build-arg CACHE_DATE="$(date)" \
                                base api

                            # Verify built images
                            for image in base api; do
                                if ! docker images | grep -q "${COMPOSE_PROJECT_NAME}_${image}"; then
                                    echo "ERROR: Image ${image} not found"
                                    exit 1
                                fi
                            done
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
                            # Start database with health monitoring
                            docker compose up -d postgres
                            
                            # Enhanced database readiness check
                            echo "Waiting for database to be ready..."
                            COUNTER=0
                            until docker compose exec -T postgres pg_isready -h localhost || [ $COUNTER -eq $DB_INIT_TIMEOUT ]; do
                                COUNTER=$((COUNTER+1))
                                echo "Attempt $COUNTER/$DB_INIT_TIMEOUT: Database not ready..."
                                sleep 2
                            done

                            if [ $COUNTER -eq $DB_INIT_TIMEOUT ]; then
                                echo "ERROR: Database failed to initialize within timeout"
                                docker compose logs postgres
                                exit 1
                            fi
                        '''
                    } catch (Exception e) {
                        error "Database setup failed: ${e.message}"
                    }
                }
            }
        }

        // ... Rest of the stages remain similar but with enhanced error handling ...
    }

    post {
        always {
            // Cleanup workspace and generate reports
            cleanWs(cleanWhenNotBuilt: false,
                   deleteDirs: true,
                   disableDeferredWipeout: true,
                   notFailBuild: true)
        }
        success {
            script {
                sh '''
                    # Generate deployment report
                    echo "=== Deployment Summary $(date) ===" > deployment_report.txt
                    echo "Build ID: ${BUILD_ID}" >> deployment_report.txt
                    echo "API URL: ${API_URL}" >> deployment_report.txt
                    docker compose ps >> deployment_report.txt
                '''
            }
        }
        failure {
            script {
                sh '''
                    # Enhanced failure debugging
                    echo "=== Failure Analysis ===" > failure_report.txt
                    docker compose ps -a >> failure_report.txt
                    docker compose logs --tail=100 >> failure_report.txt
                    
                    # Notify team (implement your notification method)
                    echo "Build failed - check failure_report.txt"
                '''
            }
        }
    }
}
