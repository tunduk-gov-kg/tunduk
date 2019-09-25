def runtime = "ubuntu.18.04-x64"
def framework = "netcoreapp2.2"
pipeline {
    agent { node { label 'master' } }
    stages {
        stage('Build') {
            steps {
                sh 'dotnet build'
            }
        }
        stage('Package') {
            steps {
                sh "dotnet deb --runtime ${runtime} --framework ${framework}"
            }
        }
    }
}