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
                timeout(time: 2, unit: 'MINUTES') {
                    sh '''
                        # Ensure .NET tools directory exists
                        mkdir -p /var/lib/jenkins/.dotnet/tools

                        # Install EF Core tools
                        dotnet tool uninstall --global dotnet-ef || true
                        dotnet tool install --global dotnet-ef --version 9.0.0

                        # Verify installations
                        dotnet --version
                        ls -la /var/lib/jenkins/.dotnet/tools
                        export PATH="/var/lib/jenkins/.dotnet/tools:$PATH"
                        dotnet ef --version || echo "EF Tools not found in PATH"

                        # List workspace contents
                        ls -la
                    '''
                }
            }
        }

        stage('Restore') {
            steps {
                timeout(time: 5, unit: 'MINUTES') {
                    sh '''
                        # List solution files
                        find . -name "*.sln"

                        # Restore dependencies
                        dotnet restore InnovateFuture.sln --verbosity minimal
                    '''
                }
            }
        }

        stage('Build Projects') {
            parallel {
                stage('Build Domain') {
                    steps {
                        sh 'dotnet build src/InnovateFuture.Domain/InnovateFuture.Domain.csproj --configuration Release --no-restore'
                    }
                }
                stage('Build Application') {
                    steps {
                        sh 'dotnet build src/InnovateFuture.Application/InnovateFuture.Application.csproj --configuration Release --no-restore'
                    }
                }
                stage('Build Infrastructure') {
                    steps {
                        sh 'dotnet build src/InnovateFuture.Infrastructure/InnovateFuture.Infrastructure.csproj --configuration Release --no-restore'
                    }
                }
                stage('Build API') {
                    steps {
                        sh 'dotnet build src/InnovateFuture.Api/InnovateFuture.Api.csproj --configuration Release --no-restore'
                    }
                }
            }
            options {
                timeout(time: 3, unit: 'MINUTES')
            }
        }

        stage('Build Tests') {
            parallel {
                stage('Build API Tests') {
                    steps {
                        sh 'dotnet build tests/InnovateFuture.Api.Tests --configuration Release --no-restore'
                    }
                }
                stage('Build Application Tests') {
                    steps {
                        sh 'dotnet build tests/InnovateFuture.Application.Tests --configuration Release --no-restore'
                    }
                }
                stage('Build Domain Tests') {
                    steps {
                        sh 'dotnet build tests/InnovateFuture.Domain.Tests --configuration Release --no-restore'
                    }
                }
            }
            options {
                timeout(time: 3, unit: 'MINUTES')
            }
        }

        stage('Run Tests') {
            parallel {
                stage('API Tests') {
                    steps {
                        sh 'dotnet test tests/InnovateFuture.Api.Tests --configuration Release --no-build'
                    }
                }
                stage('Application Tests') {
                    steps {
                        sh 'dotnet test tests/InnovateFuture.Application.Tests --configuration Release --no-build'
                    }
                }
                stage('Domain Tests') {
                    steps {
                        sh 'dotnet test tests/InnovateFuture.Domain.Tests --configuration Release --no-build'
                    }
                }
            }
            options {
                timeout(time: 3, unit: 'MINUTES')
            }
        }

        stage('Database Migration') {
            steps {
                timeout(time: 3, unit: 'MINUTES') {
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