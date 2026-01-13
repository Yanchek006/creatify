// Multi-step form for brief
class BriefForm {
    constructor() {
        this.currentStep = 1;
        this.totalSteps = 4;
        this.formData = {};
        this.init();
    }
    
    init() {
        this.setupEventListeners();
        this.updateProgress();
        this.loadSavedData();
    }
    
    setupEventListeners() {
        const nextBtn = document.getElementById('nextBtn');
        const prevBtn = document.getElementById('prevBtn');
        const submitBtn = document.getElementById('submitBtn');
        
        if (nextBtn) {
            nextBtn.addEventListener('click', () => this.nextStep());
        }
        
        if (prevBtn) {
            prevBtn.addEventListener('click', () => this.prevStep());
        }
        
        if (submitBtn) {
            submitBtn.addEventListener('click', (e) => {
                e.preventDefault();
                this.submitForm();
            });
        }
        
        // Save form data on change
        document.querySelectorAll('.brief-form input, .brief-form select, .brief-form textarea').forEach(element => {
            element.addEventListener('change', () => this.saveFormData());
        });
        
        // Color picker
        document.querySelectorAll('.color-option').forEach(option => {
            option.addEventListener('click', function() {
                document.querySelectorAll('.color-option').forEach(opt => {
                    opt.classList.remove('selected');
                });
                this.classList.add('selected');
                
                const colorInput = document.getElementById('colors');
                if (colorInput) {
                    colorInput.value = this.getAttribute('data-color');
                }
            });
        });
    }
    
    nextStep() {
        if (!this.validateCurrentStep()) {
            return;
        }
        
        this.saveFormData();
        
        if (this.currentStep < this.totalSteps) {
            // Hide current step
            document.querySelector(`.form-step[data-step="${this.currentStep}"]`).classList.remove('active');
            
            // Show next step
            this.currentStep++;
            document.querySelector(`.form-step[data-step="${this.currentStep}"]`).classList.add('active');
            
            // Update navigation buttons
            this.updateNavigation();
            this.updateProgress();
            
            // Scroll to top of form
            document.querySelector('.form-step.active').scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }
    
    prevStep() {
        if (this.currentStep > 1) {
            // Hide current step
            document.querySelector(`.form-step[data-step="${this.currentStep}"]`).classList.remove('active');
            
            // Show previous step
            this.currentStep--;
            document.querySelector(`.form-step[data-step="${this.currentStep}"]`).classList.add('active');
            
            // Update navigation buttons
            this.updateNavigation();
            this.updateProgress();
            
            // Scroll to top of form
            document.querySelector('.form-step.active').scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }
    
    validateCurrentStep() {
        const currentStepElement = document.querySelector(`.form-step[data-step="${this.currentStep}"]`);
        const requiredFields = currentStepElement.querySelectorAll('[required]');
        let isValid = true;
        
        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                isValid = false;
                field.classList.add('invalid');
                
                // Add error message if not exists
                if (!field.nextElementSibling || !field.nextElementSibling.classList.contains('error-message')) {
                    const errorMsg = document.createElement('div');
                    errorMsg.className = 'error-message';
                    errorMsg.textContent = 'Это поле обязательно для заполнения';
                    field.parentNode.insertBefore(errorMsg, field.nextSibling);
                }
            } else {
                field.classList.remove('invalid');
                const errorMsg = field.nextElementSibling;
                if (errorMsg && errorMsg.classList.contains('error-message')) {
                    errorMsg.remove();
                }
            }
        });
        
        return isValid;
    }
    
    updateNavigation() {
        const prevBtn = document.getElementById('prevBtn');
        const nextBtn = document.getElementById('nextBtn');
        const submitBtn = document.getElementById('submitBtn');
        
        if (this.currentStep === 1) {
            prevBtn.style.display = 'none';
            nextBtn.style.display = 'flex';
            submitBtn.style.display = 'none';
        } else if (this.currentStep === this.totalSteps) {
            prevBtn.style.display = 'flex';
            nextBtn.style.display = 'none';
            submitBtn.style.display = 'flex';
        } else {
            prevBtn.style.display = 'flex';
            nextBtn.style.display = 'flex';
            submitBtn.style.display = 'none';
        }
    }
    
    updateProgress() {
        const progressFill = document.querySelector('.progress-fill');
        const progressPercentage = (this.currentStep / this.totalSteps) * 100;
        
        if (progressFill) {
            progressFill.style.width = `${progressPercentage}%`;
        }
        
        // Update step indicators
        document.querySelectorAll('.progress-steps .step').forEach((step, index) => {
            const stepNumber = index + 1;
            
            if (stepNumber < this.currentStep) {
                step.classList.add('completed');
                step.classList.remove('active');
            } else if (stepNumber === this.currentStep) {
                step.classList.add('active');
                step.classList.remove('completed');
            } else {
                step.classList.remove('active', 'completed');
            }
        });
    }
    
    saveFormData() {
        const form = document.getElementById('briefForm');
        if (!form) return;
        
        const formData = new FormData(form);
        const data = {};
        
        formData.forEach((value, key) => {
            if (data[key]) {
                if (Array.isArray(data[key])) {
                    data[key].push(value);
                } else {
                    data[key] = [data[key], value];
                }
            } else {
                data[key] = value;
            }
        });
        
        // Save to localStorage
        localStorage.setItem('briefFormData', JSON.stringify(data));
        this.formData = data;
    }
    
    loadSavedData() {
        try {
            const savedData = localStorage.getItem('briefFormData');
            if (savedData) {
                const data = JSON.parse(savedData);
                this.formData = data;
                
                // Populate form fields
                Object.keys(data).forEach(key => {
                    const element = document.querySelector(`[name="${key}"]`);
                    if (element) {
                        if (element.type === 'checkbox' || element.type === 'radio') {
                            if (Array.isArray(data[key])) {
                                data[key].forEach(value => {
                                    const checkbox = document.querySelector(`[name="${key}"][value="${value}"]`);
                                    if (checkbox) checkbox.checked = true;
                                });
                            } else {
                                const radio = document.querySelector(`[name="${key}"][value="${data[key]}"]`);
                                if (radio) radio.checked = true;
                            }
                        } else {
                            element.value = data[key];
                        }
                    }
                });
            }
        } catch (error) {
            console.error('Error loading saved form data:', error);
        }
    }
    
    submitForm() {
        if (!this.validateCurrentStep()) {
            return;
        }
        
        this.saveFormData();
        
        // Show loading state
        const submitBtn = document.getElementById('submitBtn');
        const originalText = submitBtn.innerHTML;
        submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Отправка...';
        submitBtn.disabled = true;
        
        // Simulate API call
        setTimeout(() => {
            // Success
            this.showSuccess();
            
            // Clear saved data
            localStorage.removeItem('briefFormData');
            this.formData = {};
            
            // Reset form
            document.getElementById('briefForm').reset();
            
            // Reset to step 1
            document.querySelectorAll('.form-step').forEach(step => {
                step.classList.remove('active');
            });
            document.querySelector('.form-step[data-step="1"]').classList.add('active');
            this.currentStep = 1;
            this.updateNavigation();
            this.updateProgress();
            
            // Restore button
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
            
        }, 2000);
    }
    
    showSuccess() {
        // Create success overlay
        const successOverlay = document.createElement('div');
        successOverlay.className = 'success-overlay glass-card';
        successOverlay.innerHTML = `
            <div class="success-content">
                <div class="success-icon">
                    <i class="fas fa-check-circle"></i>
                </div>
                <h2>Бриф успешно отправлен!</h2>
                <p>Мы свяжемся с вами в течение 24 часов для обсуждения деталей проекта.</p>
                <p>Наш менеджер Роберт позвонит вам по указанному телефону.</p>
                <div class="success-actions">
                    <a href="index.html" class="btn-secondary">На главную</a>
                    <button class="btn-primary" id="closeSuccess">Закрыть</button>
                </div>
            </div>
        `;
        
        document.body.appendChild(successOverlay);
        
        // Add confetti
        if (typeof confetti === 'function') {
            confetti({
                particleCount: 150,
                spread: 70,
                origin: { y: 0.6 }
            });
        }
        
        // Close button
        document.getElementById('closeSuccess').addEventListener('click', () => {
            successOverlay.remove();
        });
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new BriefForm();
});