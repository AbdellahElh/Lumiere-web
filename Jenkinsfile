pipeline {
    agent {label 'appserverVM'}
    environment {

        DOCKER_IMAGE = 'karimdm8/dotnetapp'
        CONTAINER_NAME = 'dotnet_application'
    }

    stages {
        stage('Checkout') {
            steps {
               git branch: 'main', credentialsId: 'keycontainer', url: 'git@github.com:HOGENT-RISE/dotnet-2425-gent5.git'
            }
        }

        stage('Cleanup Docker') {
            steps {
                sh 'docker system prune -f'
            }
        }

        stage('Docker Build') {
            steps {
                script {
                    // Maak de Docker-image
                    sh "docker build --no-cache -t ${DOCKER_IMAGE} ."
                }
            }
        }

        stage('Docker Push') {
            steps {
                script {
                    // Push de Docker-image naar Docker Hub
                    sh "docker push ${DOCKER_IMAGE}"
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    // Stop de huidige container als deze draait
                    sh "docker stop ${CONTAINER_NAME} || true"
                    sh "docker rm ${CONTAINER_NAME} || true"

                    // Start de nieuwe container met HTTPS configuratie
                    sh """
                    docker run -d -p 80:8080 \
                        --name ${CONTAINER_NAME} \
                        ${DOCKER_IMAGE}
                    """
                }
            }
        }
    }
}
