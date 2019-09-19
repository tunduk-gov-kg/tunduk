/*#!groovy

try {
    node("") {
        echo "Hello"
        dotnet build
    }
} catch (e) {
        throw e
    } finally {
        // Success or failure, always send notifications
        echo "Success"
    }
*/

pipeline {
    agent { node { label 'master' } }
    stages {
        stage('check') {
            steps {
                sh 'dotnet --info'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build'
            }
        }
    }
}


