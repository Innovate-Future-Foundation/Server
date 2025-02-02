import java.text.SimpleDateFormat
import java.net.URLEncoder
SimpleDateFormat dayFormat = new SimpleDateFormat('yyyy/MM/dd HH:mm:ss');
def nowTimesamp = dayFormat.format(new Date());

pipeline {
    agent any
    environment {
        CODE_REPO_URL = 'https://github.com/Innovate-Future-Foundation/Server'
        AWS_CONFIGURE_REGION = 'ap-southeast-2'
        ECR_URL = '058264518385.dkr.ecr.ap-southeast-2.amazonaws.com'
        BACKEND_API_ECR_REPO = 'inff/backend-api'
        BACKEND_BUILD_ECR_REPO = 'inff/backend-build'
        DATABASE_PORT = '5432'
        DATABASE_NAME = 'InnovateFuture'
        DATABASE_CREDENTIALS = credentials('postgres_user')
        JWTConfig__SecretKey = 'MY_SECRET_KEY'
        ASPNETCORE_ENVIRONMENT = 'Development'
        
    }
    // parameters {
        
    // }
    stages {
        stage ('Prepare') {
            steps {
                script {
                    echo "=== Prepare CICD Pipeline #${currentBuild.number} ==="
                    env.DATABASE_CONN = "Host=localhost;Port=5432;Database=${DATABASE_NAME};Username=${DATABASE_CREDENTIALS_USR};Password=${DATABASE_CREDENTIALS_PSW}"

                    env.GIT_COMMIT_HASH = sh (
                        script: 'git rev-parse --short HEAD',
                        returnStdout: true
                    ).trim()
                    env.CURRENT_TAG = GIT_COMMIT_HASH + '_' + currentBuild.number
                    echo "=== Complete Preparation #${currentBuild.number} ==="
                }
            }
        }
        stage('Build') {
            steps {
                script {
                    echo "=== Start Building Image #${currentBuild.number} ==="
                    echo "- Build Base Image"
                    sh ("""
                        docker build -f Dockerfile.base \
                        -t inff-api-build \
                        .
                    """)

                    echo "- Build API Image"
                    sh ("""
                        docker build -f Dockerfile.api \
                        -t inff-api .
                    """)

                    echo "=== Complete Building #${currentBuild.number} ==="
                }
            }
        }
        stage('Push Backend Image') {
            steps {
                script {
                    echo "=== Start Pushing Image ==="
                    echo "- Login to ECR ${ECR_URL}"
                    withAWS(credentials: 'uat_ci_access_key', region: env.AWS_CONFIGURE_REGION) {
                        sh "aws ecr get-login-password --region ${AWS_CONFIGURE_REGION} | docker login --username AWS --password-stdin ${ECR_URL}"
                    }

                    echo "- Tag images: ${env.CURRENT_TAG}"
                    sh "docker tag inff-api-build:latest ${ECR_URL}/${BACKEND_BUILD_ECR_REPO}:${env.CURRENT_TAG}"
                    sh "docker tag inff-api:latest ${ECR_URL}/${BACKEND_API_ECR_REPO}:${env.CURRENT_TAG}"

                    echo "- Push image ${BACKEND_BUILD_ECR_REPO} to ECR"
                    sh "docker push ${ECR_URL}/${BACKEND_BUILD_ECR_REPO}:${env.CURRENT_TAG}"
                    echo "- Push image ${BACKEND_API_ECR_REPO} to ECR"
                    sh "docker push ${ECR_URL}/${BACKEND_API_ECR_REPO}:${env.CURRENT_TAG}"

                    echo "=== Complete Pushing Image ==="
                }
            }
        }
        stage('Deploy Backend Image') {
            steps {
                script {
                    echo "=== Deploying backend image to ECS ==="
                    withAWS(credentials: 'uat_cd_access_key', region: env.AWS_CONFIGURE_REGION) {
                        try {
                            echo "- Getting Current Task Definition"
                            def currentTaskDef = sh(
                                script: "aws ecs describe-task-definition --task-definition inff-uat-backend-db-set --query 'taskDefinition'",
                                returnStdout: true
                            ).trim()
                            
                            echo "- Modifying Task Definition with new image tag"
                            def updatedTaskDef = readJSON(text: currentTaskDef)
                            updatedTaskDef.containerDefinitions.each { container ->
                                if (container.name == "api") {
                                    container.image = "${env.ECR_URL}/${BACKEND_API_ECR_REPO}:${env.CURRENT_TAG}"
                                } else if (container.name == "migration") {
                                    container.image = "${env.ECR_URL}/${BACKEND_BUILD_ECR_REPO}:${env.CURRENT_TAG}"
                                }
                            }
                            
                            echo "- Creating New Task Definition with updatedTaskDef"
                            def newTaskDef = sh(
                                script: "aws ecs register-task-definition --cli-input-json '${JSONSerializer.toString(updatedTaskDef)}'",
                                returnStdout: true
                            ).trim()
                            
                            def newTaskDefArn = readJSON(text: newTaskDef).taskDefinition.taskDefinitionArn
                            
                            // Update ECS service
                            echo "- Updating ECS service with new task definition"
                            sh """
                                aws ecs update-service \
                                --cluster inff-uat-cluster \
                                --service inff-uat-backend-srv \
                                --task-definition ${newTaskDefArn} \
                                --force-new-deployment
                            """
                            
                            // Wait for service stabilization
                            echo "Waiting for service stabilization..."
                            sh """
                                aws ecs wait services-stable \
                                --cluster inff-uat-cluster \
                                --services inff-uat-backend-srv
                            """
                            
                        } catch (Exception ex) {
                            error "Deployment failed: ${ex.getMessage()}"
                            // Add automatic rollback logic here if needed
                        }
                    }
                }
            }
        }
    }

    post {
        success {
            echo "*** Deployment completed successfully ***"
            // todo notification
        }
        failure {
            echo "!!! Deployment failed !!!"
            // todo rollback
        }
        always {
            echo "Pipeline finished at ${nowTimesamp}"
            echo 'Clean Workspace...'
            cleanWs()
        }
    }
}
