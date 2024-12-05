// Configuration de la scène Three.js
const container = document.getElementById('model-container');
const scene = new THREE.Scene();
scene.background = new THREE.Color('rgb(32, 62, 112)'); // Ajout d'un fond gris clair

// Ajustement du ratio de la caméra
const width = container.clientWidth;
const height = container.clientHeight;
const camera = new THREE.PerspectiveCamera(27, width / height, 0.1, 1000);

// Configuration du renderer
const renderer = new THREE.WebGLRenderer({
    antialias: true,
    alpha: true
});
renderer.setPixelRatio(window.devicePixelRatio);
container.appendChild(renderer.domElement);

// Éclairage amélioré
const ambientLight = new THREE.AmbientLight(0xffffff, 0.7);
scene.add(ambientLight);

const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
directionalLight.position.set(5, 5, 5);
scene.add(directionalLight);

// Ajout d'une lumière d'appoint
const pointLight = new THREE.PointLight(0xffffff, 0.5);
pointLight.position.set(-5, 5, -5);
scene.add(pointLight);

// Configuration des contrôles
const controls = new THREE.OrbitControls(camera, renderer.domElement);
controls.enableDamping = true; // Ajoute de l'inertie aux contrôles
controls.enableZoom = false;
controls.dampingFactor = 0.05;

controls.rotateSpeed = 0.3;  // Réduit la vitesse de rotation (défaut: 1.0)
controls.minPolarAngle = Math.PI / 4;  // Limite l'angle vertical minimum
controls.maxPolarAngle = Math.PI / 1.5; // Limite l'angle vertical maximum
controls.minAzimuthAngle = -Math.PI / 2; // Limite la rotation horizontale minimum
controls.maxAzimuthAngle = Math.PI / 2;  // Limite la rotation horizontale maximum
controls.minAzimuthAngle = -Infinity; // Pas de limite minimale
controls.maxAzimuthAngle = Infinity;
controls.minPolarAngle = -Infinity;
controls.maxPolarAngle = Infinity;

// Position initiale de la caméra
camera.position.set(0, 0, 5);
controls.update();

// Créer un groupe pour le modèle
const modelGroup = new THREE.Group();
scene.add(modelGroup);

let autoRotate = true;

// Fonction d'animation
function animate() {
    requestAnimationFrame(animate);

    // Ajout des gestionnaires d'événements pour la souris
    renderer.domElement.addEventListener('mousedown', () => {
        autoRotate = false;
    });

    renderer.domElement.addEventListener('mouseup', () => {
        autoRotate = true;
    });

    if (autoRotate) {
        modelGroup.rotation.y -= 0.005;
    }

    controls.update();
    renderer.render(scene, camera);
}

// Démarrage de l'animation
animate();

// Chargement du modèle GLB
const loader = new THREE.GLTFLoader();

console.log('Début du chargement du modèle...');

loader.load(
    '/models/ballon.glb',
    (gltf) => {
        console.log('Modèle chargé avec succès:', gltf);

        const model = gltf.scene;

        // Calculer la boîte englobante
        const box = new THREE.Box3().setFromObject(model);
        const center = box.getCenter(new THREE.Vector3());
        const size = box.getSize(new THREE.Vector3());

        console.log('Taille du modèle:', size);
        console.log('Centre du modèle:', center);

        // Ajuster la position du modèle pour qu'il soit centré
        model.position.sub(center);

        // Ajouter le modèle au groupe
        modelGroup.add(model);

        console.log('Modèle ajouté à la scène');
    },
    (xhr) => {
        console.log((xhr.loaded / xhr.total * 100) + '% chargé');
    },
    (error) => {
        console.error('Erreur lors du chargement:', error);
    }
);

// Gestion du redimensionnement
window.addEventListener('resize', () => {
    const newWidth = container.clientWidth;
    const newHeight = container.clientHeight;

    camera.aspect = newWidth / newHeight;
    camera.updateProjectionMatrix();

    renderer.setSize(newWidth, newHeight);
});

// Initialisation de la taille du renderer
function initRendererSize() {
    const newWidth = container.clientWidth;
    const newHeight = container.clientHeight;

    camera.aspect = newWidth / newHeight;
    camera.updateProjectionMatrix();

    renderer.setSize(newWidth, newHeight);
}

// Appel initial pour définir la taille du renderer
initRendererSize();