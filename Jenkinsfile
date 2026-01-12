pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/tmp/dotnet-cli-home'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        DOCKER_IMAGE_NAME = 'dosfinal-api'
        DOCKER_REGISTRY = 'your-registry.azurecr.io' // Alterar para o seu registry
        SONAR_HOST_URL = 'http://localhost:9000' // Alterar para o URL do SonarQube
        SONAR_PROJECT_KEY = 'DOSFinal'
        KUBECONFIG_CREDENTIALS = 'kubeconfig-credentials'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
                echo 'Source code checked out successfully'
            }
        }

        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore'
                echo 'Dependencies restored successfully'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build -c Release --no-restore'
                echo 'Build completed successfully'
            }
        }

        stage('Run Unit Tests') {
            steps {
                sh 'dotnet test -c Release --no-build --logger "trx;LogFileName=test-results.trx" --collect:"XPlat Code Coverage"'
                echo 'Unit tests completed'
            }
            post {
                always {
                    // Publish test results
                    script {
                        def testResults = findFiles(glob: '**/test-results.trx')
                        if (testResults.length > 0) {
                            echo "Found ${testResults.length} test result file(s)"
                        }
                    }
                }
            }
        }

        stage('SonarQube Analysis') {
            environment {
                SONAR_TOKEN = credentials('sonar-token')
            }
            steps {
                script {
                    withSonarQubeEnv('SonarQube') {
                        sh """
                            dotnet sonarscanner begin \
                                /k:"${SONAR_PROJECT_KEY}" \
                                /d:sonar.host.url="${SONAR_HOST_URL}" \
                                /d:sonar.token="${SONAR_TOKEN}" \
                                /d:sonar.cs.opencover.reportsPaths=**/coverage.opencover.xml
                            
                            dotnet build -c Release --no-restore
                            
                            dotnet sonarscanner end /d:sonar.token="${SONAR_TOKEN}"
                        """
                    }
                }
            }
        }

        stage('Quality Gate') {
            steps {
                timeout(time: 5, unit: 'MINUTES') {
                    waitForQualityGate abortPipeline: true
                }
                echo 'Quality gate passed'
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    def imageTag = "${DOCKER_IMAGE_NAME}:${env.BUILD_NUMBER}"
                    def imageLatest = "${DOCKER_IMAGE_NAME}:latest"
                    
                    sh "docker build -t ${imageTag} -t ${imageLatest} ."
                    
                    echo "Docker image built: ${imageTag}"
                }
            }
        }

        stage('Push Docker Image') {
            when {
                branch 'master'
            }
            steps {
                script {
                    withCredentials([usernamePassword(
                        credentialsId: 'docker-registry-credentials',
                        usernameVariable: 'DOCKER_USER',
                        passwordVariable: 'DOCKER_PASS'
                    )]) {
                        sh "echo ${DOCKER_PASS} | docker login ${DOCKER_REGISTRY} -u ${DOCKER_USER} --password-stdin"
                        
                        def imageTag = "${DOCKER_REGISTRY}/${DOCKER_IMAGE_NAME}:${env.BUILD_NUMBER}"
                        def imageLatest = "${DOCKER_REGISTRY}/${DOCKER_IMAGE_NAME}:latest"
                        
                        sh "docker tag ${DOCKER_IMAGE_NAME}:${env.BUILD_NUMBER} ${imageTag}"
                        sh "docker tag ${DOCKER_IMAGE_NAME}:latest ${imageLatest}"
                        
                        sh "docker push ${imageTag}"
                        sh "docker push ${imageLatest}"
                        
                        echo "Docker image pushed to registry: ${imageTag}"
                    }
                }
            }
        }

        stage('Deploy to Kubernetes') {
            when {
                branch 'master'
            }
            steps {
                script {
                    withCredentials([file(credentialsId: "${KUBECONFIG_CREDENTIALS}", variable: 'KUBECONFIG')]) {
                        // Deploy using Helm
                        sh """
                            helm upgrade --install dosfinal ./helm/dosfinal \
                                --namespace dosfinal \
                                --create-namespace \
                                --set image.repository=${DOCKER_REGISTRY}/${DOCKER_IMAGE_NAME} \
                                --set image.tag=${env.BUILD_NUMBER} \
                                --wait \
                                --timeout 5m
                        """
                        
                        echo "Deployment to Kubernetes completed"
                    }
                }
            }
        }
    }

    post {
        always {
            // Clean up workspace
            cleanWs()
            
            // Remove dangling Docker images
            sh 'docker image prune -f || true'
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
            // Add notification here (email, Slack, etc.)
        }
    }
}
