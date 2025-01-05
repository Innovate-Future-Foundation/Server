# EC2 Jenkins Pipeline

This repository contains the Jenkins pipeline configuration for deploying a .NET application to an EC2 instance using Docker.

## Pipeline Overview

The Jenkins pipeline defined in `.devops/dev/EC2.JenkinsFile` performs the following steps:

1. **Prepare**: Cleans the workspace, sets up the Docker host, and configures the database connection.
2. **Checkout**: Checks out the code from the specified Git repository and branch.
3. **Build**: Builds the Docker image for the application.
4. **Push Image**: Tags and pushes the Docker image to Amazon ECR.
5. **Clean Up**: Stops and removes any existing Docker containers and images.
6. **Connect Remote Docker Host**: Sets up the remote Docker host and network.
7. **Deploy**: Deploys the Docker container to the remote Docker host.
8. **Verify**: Verifies that the container is running and accessible.
9. **Post Actions**: Sends a notification to Discord with the build and deployment details.

## Environment Variables

The following environment variables are used in the pipeline:

- `CODE_REPO_URL`: URL of the Git repository.
- `BRANCH_NAME`: Branch name to checkout.
- `BASE_DIRECTORY`: Base directory for the Docker Compose files.
- `AWS_CONFIGURE_REGION`: AWS region for ECR.
- `ECR_URL`: URL of the Amazon ECR repository.
- `ECR_REPO`: Name of the ECR repository.
- `DOCKER_IP`: IP address of the Docker host.
- `DOCKER_NETWORK`: Docker network name.
- `DATABASE_PORT`: Port for the database.
- `DATABASE_NAME`: Name of the database.
- `DATABASE_CREDENTIALS`: Credentials for the database.
- `JWTConfig__SecretKey`: Secret key for JWT configuration.
- `ASPNETCORE_ENVIRONMENT`: ASP.NET Core environment.

## Steps to Run the Pipeline

1. **Set Up Jenkins**: Ensure Jenkins is set up with the necessary plugins and credentials.
2. **Configure Environment Variables**: Set the required environment variables in Jenkins.
3. **Run the Pipeline**: Trigger the pipeline from Jenkins.

## Notifications

The pipeline sends a notification to a Discord webhook with the build and deployment details, including the job duration, triggered by user, Git commit details, ECR image tag, and health check URLs.

## Health Check

The health check URL for the deployed application is:

- [Swagger](http://${env.DOCKER_IP}:5091/swagger/index.html)
- [pgAdmin 4](http://${env.DOCKER_IP}:5050)

## License
