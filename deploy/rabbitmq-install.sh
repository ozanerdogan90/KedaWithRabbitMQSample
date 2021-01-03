helm repo add bitnami https://charts.bitnami.com/bitnami
helm install my-rabbitmq \
  --set auth.username=<YOURUSERNAME>,auth.password=<YOURPASSWORD>,auth.erlangCookie=secretcookie \
    bitnami/rabbitmq