import java.text.SimpleDateFormat
import java.net.URLEncoder
SimpleDateFormat dayFormat = new SimpleDateFormat('yyyy/MM/dd HH:mm:ss');
def nowTimesamp = dayFormat.format(new Date());

pipeline {
    agent any
    environment {
        CODE_REPO_URL = 'https://github.com/Innovate-Future-Foundation/Server'
        BRANCH_NAME = 'devops/uat'
        BASE_DIRECTORY = './jenkins'
        AWS_CONFIGURE_REGION = 'ap-southeast-2'
        ECR_URL = '058264518385.dkr.ecr.ap-southeast-2.amazonaws.com'
        BACKEND_API_ECR_REPO = 'inff/backend-api'
        BACKEND_BUILD_ECR_REPO = 'inff/backend-build'
        API_TEST_ECR_REPO = 'inff/api-test'
        DOCKER_NETWORK = "inff-network-backend"
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
                    echo 'Clean Workspace...'
                    cleanWs()

                    echo 'Setup Prerequisites...'
                    env.DATABASE_CONN = "Host=localhost;Port=5432;Database=${DATABASE_NAME};Username=${DATABASE_CREDENTIALS_USR};Password=${DATABASE_CREDENTIALS_PSW}"
                }
            }
        }
        stage('Build') {
            steps {
                script {
                    echo '=== Start Building Docker Image ==='
                    echo '- Building Base Image'
                    sh ("""
                        docker build -f Dockerfile.base \
                        -t inff-api-build \
                        .
                    """)

                    echo '- Building API Image'
                    sh ("""
                        docker build -f Dockerfile.api \
                        -t inff-api \
                        --build-arg DBConnection="${env.DATABASE_CONN}" \
                        --build-arg JWTConfig__SecretKey="${JWTConfig__SecretKey}" \
                        --build-arg ASPNETCORE_ENVIRONMENT="${ASPNETCORE_ENVIRONMENT}" \
                        --build-arg ASPNETCORE_URLS="http://+:5091" \
                        .
                    """)
                    // sh ("""
                    //     docker build -f Dockerfile.test \
                    //     -t inff-api-test \
                    //     .
                    // """)
                    echo '=== Building complete #${currentBuild.number} ==='
                }
            }
        }
        stage('Push Image') {
            steps {
                script {
                    echo "Login to ECR ${ECR_URL}"
                    withAWS(credentials: 'uat_ci_access_key', region: env.AWS_CONFIGURE_REGION) {
                        sh "aws ecr get-login-password --region ${AWS_CONFIGURE_REGION} | docker login --username AWS --password-stdin ${ECR_URL}"
                    }
                    GIT_COMMIT_HASH = sh (
                        script: 'git rev-parse --short HEAD',
                        returnStdout: true
                    ).trim()
                    env.CURRENT_TAG = GIT_COMMIT_HASH + '_' + currentBuild.number
                    echo "Tag: ${env.CURRENT_TAG}"
                    sh "docker tag inff-api-build:latest ${ECR_URL}/${BACKEND_BUILD_ECR_REPO}:${env.CURRENT_TAG}"
                    sh "docker tag inff-api:latest ${ECR_URL}/${BACKEND_API_ECR_REPO}:${env.CURRENT_TAG}"
                    // sh "docker tag inff-api-test:latest ${ECR_URL}/${API_TEST_ECR_REPO}:${env.CURRENT_TAG}"

                    echo "Push image to ECR"
                    sh "docker push ${ECR_URL}/${API_BUILD_ECR_REPO}:${env.CURRENT_TAG}"
                    sh "docker push ${ECR_URL}/${API_ECR_REPO}:${env.CURRENT_TAG}"
                    // sh "docker push ${ECR_URL}/${API_TEST_ECR_REPO}:${env.CURRENT_TAG}"
                }
            }
        }
    }
    post {
        always {
            echo "Pipeline finished at ${nowTimesamp}"
        }
    }
}
