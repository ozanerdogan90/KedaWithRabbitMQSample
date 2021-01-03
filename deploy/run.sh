kubectl apply -f producer-deployment.yaml
kubectl apply -f producer-service.yaml
kubectl apply -f producer-ingress.yaml

kubectl apply -f consumer-deployment.yaml

kubectl apply -f host-alias-tweaker.yaml