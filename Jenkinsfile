#!groovy

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



