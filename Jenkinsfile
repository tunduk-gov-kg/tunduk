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
                sh '/usr/bin/dotnet deb -r ubuntu\\.18\\.04-x64 -f netcoreapp2\\.2'
            }
        }
    }
}