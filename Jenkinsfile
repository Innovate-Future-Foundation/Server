pipeline {
    agent any

    options {
        timeout(time: 10, unit: 'MINUTES')
    }

    environment {
        DOTNET_SDK_VERSION = '8.0'
        PROJECT_NAME = 'InnovateFuture'
        DOCKER_IMAGE_NAME = 'innovatefuture-api'
        AWS_REGION = 'us-east-1'
        DOTNET_CLI_HOME = "/var/lib/jenkins/.dotnet"
        DOTNET_NOLOGO = 'true'
        DOTNET_CLI_TELEMETRY_OPTOUT = 'true'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        PATH = "/var/lib/jenkins/.dotnet/tools:$PATH"
    }

    stages {
        stage('Checkout') {
            steps {
                cleanWs()
                checkout scm
            }
        }

        stage('Setup') {
            steps {
                timeout(time: 1, unit: 'MINUTES') {
                    sh '''
                        mkdir -p /var/lib/jenkins/.dotnet/tools
                        dotnet tool uninstall --global dotnet-ef || true
                        dotnet tool install --global dotnet-ef --version 9.0.0
                        dotnet --version
                    '''
                }
            }
        }

        stage('Build Solution') {
            steps {
                timeout(time: 7, unit: 'MINUTES') {
                    sh '''
                        # Restore with minimal verbosity
                        dotnet restore InnovateFuture.sln --verbosity minimal

                        # Build with optimizations
                        dotnet build InnovateFuture.sln \
                            --configuration Release \
                            --no-restore \
                            --verbosity minimal \
                            /p:ContinuousIntegrationBuild=true \
                            /maxcpucount:1
                    '''
                }
            }
        }

        stage('Run Tests') {
            steps {
                timeout(time: 1, unit: 'MINUTES') {
                    sh '''
                        dotnet test InnovateFuture.sln \
                            --configuration Release \
                            --no-build \
                            --verbosity minimal
                    '''
                }
            }
        }

        stage('Database Migration') {
            steps {
                timeout(time: 1, unit: 'MINUTES') {
                    script {
                        def infrastructureProject = "${WORKSPACE}/src/InnovateFuture.Infrastructure/InnovateFuture.Infrastructure.csproj"
                        def apiProject = "${WORKSPACE}/src/InnovateFuture.Api/InnovateFuture.Api.csproj"

                        sh """
                            dotnet ef database update \
                                --project "${infrastructureProject}" \
                                --startup-project "${apiProject}" \
                                -- --environment "Development"
                        """
                    }
                }
            }
        }
    }

    post {
        success {
            echo '✅ Build successful! All stages completed.'
        }
        failure {
            echo '❌ Build failed! Check the logs above for errors.'
        }
        always {
            cleanWs()
        }
    }
}