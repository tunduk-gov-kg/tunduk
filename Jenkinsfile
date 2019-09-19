#!groovy

notifyBuildDetails = ""

try {
    String debFileName = "management-${env.BRANCH_NAME}.deb"
    
    node("master") {
        deleteDir()
        echo "Hello"
        
    }
} catch (e) {
        currentBuild.result = "FAILED"
        throw e
    } finally {
        // Success or failure, always send notifications
        notifyBuild(currentBuild.result, notifyBuildDetails)
    }



