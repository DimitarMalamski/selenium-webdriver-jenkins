pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Check .NET SDK') {
            steps {
                bat '''
                dotnet --version
                '''
            }
        }

        stage('Restore') {
            steps {
                bat '''
                dotnet restore
                '''
            }
        }

        stage('Build') {
            steps {
                bat '''
                dotnet build --configuration Release --no-restore
                '''
            }
        }

        stage('Test Project 1') {
            steps {
                bat '''
                dotnet test .\\TestProject1\\TestProject1.csproj --configuration Release --no-build
                '''
            }
        }

        stage('Test Project 2') {
            steps {
                bat '''
                dotnet test .\\TestProject2\\TestProject2.csproj --configuration Release --no-build
                '''
            }
        }

        stage('Test Project 3') {
            steps {
                bat '''
                dotnet test .\\TestProject3\\TestProject3.csproj --configuration Release --no-build
                '''
            }
        }
    }
}