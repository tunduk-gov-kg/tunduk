#!groovy

try {
    String debFileName = "management-${env.BRANCH_NAME}.deb"
    
    node("master") {
        echo "Hello"
        dotnet build
    }
} catch (e) {
        throw e
    } finally {
        // Success or failure, always send notifications
        echo "Success"
    }



