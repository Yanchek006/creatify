// 3D Background with Three.js
let scene, camera, renderer, controls;
let objects = [];
let mouseX = 0, mouseY = 0;
let windowHalfX = window.innerWidth / 2;
let windowHalfY = window.innerHeight / 2;

function init3D() {
    scene = new THREE.Scene();
    
    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    camera.position.z = 15;
    
    renderer = new THREE.WebGLRenderer({ 
        alpha: true, 
        antialias: true 
    });
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setPixelRatio(window.devicePixelRatio);
    document.getElementById('canvas3d').appendChild(renderer.domElement);
    
    // Add lighting
    const ambientLight = new THREE.AmbientLight(0x333333);
    scene.add(ambientLight);
    
    const directionalLight = new THREE.DirectionalLight(0xffffff, 0.7);
    directionalLight.position.set(5, 5, 5);
    scene.add(directionalLight);
    
    // Create floating geometric shapes
    const geometryTypes = [
        new THREE.IcosahedronGeometry(1, 0),
        new THREE.OctahedronGeometry(1.2, 0),
        new THREE.TorusGeometry(0.8, 0.3, 16, 100),
        new THREE.ConeGeometry(1, 2, 8),
        new THREE.TetrahedronGeometry(1.3, 0)
    ];
    
    const colors = [0x667eea, 0x764ba2, 0xf093fb, 0x4facfe, 0x00f2fe];
    
    for (let i = 0; i < 15; i++) {
        const geometry = geometryTypes[Math.floor(Math.random() * geometryTypes.length)];
        const material = new THREE.MeshPhongMaterial({
            color: colors[Math.floor(Math.random() * colors.length)],
            shininess: 100,
            transparent: true,
            opacity: 0.3,
            wireframe: Math.random() > 0.7
        });
        
        const object = new THREE.Mesh(geometry, material);
        
        // Random position
        object.position.x = Math.random() * 30 - 15;
        object.position.y = Math.random() * 20 - 10;
        object.position.z = Math.random() * 20 - 20;
        
        // Random rotation
        object.rotation.x = Math.random() * Math.PI;
        object.rotation.y = Math.random() * Math.PI;
        
        // Random size
        const scale = Math.random() * 1.5 + 0.5;
        object.scale.set(scale, scale, scale);
        
        // Store original position for animation
        object.userData = {
            originalX: object.position.x,
            originalY: object.position.y,
            originalZ: object.position.z,
            speedX: Math.random() * 0.02 - 0.01,
            speedY: Math.random() * 0.02 - 0.01,
            speedZ: Math.random() * 0.02 - 0.01
        };
        
        scene.add(object);
        objects.push(object);
    }
    
    // Mouse move event for parallax
    document.addEventListener('mousemove', onDocumentMouseMove);
    window.addEventListener('resize', onWindowResize);
    
    animate();
}

function onDocumentMouseMove(event) {
    mouseX = (event.clientX - windowHalfX) * 0.001;
    mouseY = (event.clientY - windowHalfY) * 0.001;
}

function onWindowResize() {
    windowHalfX = window.innerWidth / 2;
    windowHalfY = window.innerHeight / 2;
    
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
}

function animate() {
    requestAnimationFrame(animate);
    
    // Rotate camera based on mouse position
    camera.position.x += (mouseX * 5 - camera.position.x) * 0.05;
    camera.position.y += (-mouseY * 5 - camera.position.y) * 0.05;
    camera.lookAt(scene.position);
    
    // Animate objects
    objects.forEach(object => {
        object.rotation.x += 0.005;
        object.rotation.y += 0.005;
        
        // Floating animation
        object.position.x = object.userData.originalX + Math.sin(Date.now() * 0.001 + object.userData.originalX) * 2;
        object.position.y = object.userData.originalY + Math.cos(Date.now() * 0.001 + object.userData.originalY) * 1.5;
        
        // Pulsing scale
        const pulse = Math.sin(Date.now() * 0.001 + object.userData.originalZ) * 0.1 + 1;
        object.scale.set(pulse, pulse, pulse);
    });
    
    renderer.render(scene, camera);
}

// Initialize when page loads
if (document.getElementById('canvas3d')) {
    init3D();
}