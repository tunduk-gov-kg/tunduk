#!groovy

try {
    String debFileName = "management-${env.BRANCH_NAME}.deb"
    
    node("master") {
        deleteDir()
        echo "Hello"
        
    }
} catch (e) {
        throw e
    } finally {
        // Success or failure, always send notifications
        echo "Success"
    }



