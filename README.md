# DCR Kubernetes Blueprint (Dog API)

Dette repo er et (PoC) bygget for at demonstrere, hvordan vi kan køre et robust og sikkert .Net/PostgreSQL-miljø i Kubernetes
Selve applikationen er et simpelt "Dog API", men strukturen i "k8s/production.yaml" fungerer som en skabelon, der kan
genbruges til DCR's rigtige services.

#Arkitektur &  Best practices
Skabelonen demonstrerer følgende koncepter sat til produktionsbrug:

-Seperation of Concerns: Konfiguration ("ConfigMap") og adgangskoder/connection strings ("secret") er trukket ud af selve
applikationen.
-Data Persistence: Databasen benytter en Persistent Volume Claim (PVC), så data overlever pod-crashes og genstarter.
-Self-healing:** API'en overvåges af `livenessProbe` og `readinessProbe`. Hvis applikationen fryser, genstarter Kubernetes
den automatisk.
-Resource Limits:** Der er sat hard-limits på RAM og CPU (requests/limits) for at forhindre, at en enkelt service med et
et memory leak overbelaster clusteret (Kubernetes miljøet).
-Ingress: Trafik udefra styres via en Ingress Controller, der lytter på `dogapi.local`.
-CI/CD: En simpel GitHub Action bygger og uploader automatisk et nyt Docker-image ved hvert push til `main`.

#Sådan kan det køres lokalt "Minikube"
Første forudsætning:- at Minikube og `kubectl` er installeret.
-Du har tilføjet din Minikube IP til din hosts-fil (`/etc/hosts` på Linux/Mac eller `C:\Windows\System32\drivers\etc\hosts` på Windows):
`192.168.49.2 dogapi.local` *(husk at tjekke din aktuelle ip med `minikube ip`)

Derefter start Kubernetes miljøet
Start Minikube og åbn Ingress-tunnelen (skal køre i baggrunden):
minikube start
minikube tunnel

3. Deploy infrastrukturen med denne kommando:
kubectl apply -f k8s/production.yaml
derefter kan man indstaste denne kommando til at følge med i pods'ne:
kubectl get pods -w

4. Test
indtast denne kommand, API rammes via en browser eller url:
curl -H "Host: dogapi.local" http://<din-minikube-ip>/dogs 
