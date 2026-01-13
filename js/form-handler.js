// Form handler for all forms
class FormHandler {
    constructor() {
        this.forms = [];
        this.init();
    }
    
    init() {
        // Find all forms with data-form attribute
        const forms = document.querySelectorAll('form[data-form]');
        forms.forEach(form => {
            this.registerForm(form);
        });
    }
    
    registerForm(form) {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const formType = form.getAttribute('data-form');
            const formData = new FormData(form);
            const submitBtn = form.querySelector('button[type="submit"]');
            const originalBtnText = submitBtn.innerHTML;
            
            // Show loading state
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Отправка...';
            submitBtn.disabled = true;
            
            try {
                // In production, this would be a real API call
                // For demo purposes, we'll simulate success
                
                // Simulate API delay
                await new Promise(resolve => setTimeout(resolve, 1500));
                
                // Success handling
                this.showSuccess(form);
                
                // Reset form
                form.reset();
                
                // Send to analytics (optional)
                this.trackFormSubmission(formType, formData);
                
            } catch (error) {
                this.showError(form, 'Ошибка при отправке. Попробуйте еще раз.');
                console.error('Form submission error:', error);
                
            } finally {
                // Restore button
                submitBtn.innerHTML = originalBtnText;
                submitBtn.disabled = false;
            }
        });
    }
    
    showSuccess(form) {
        // Create success message
        const successDiv = document.createElement('div');
        successDiv.className = 'form-success glass-card';
        successDiv.innerHTML = `
            <div class="success-content">
                <i class="fas fa-check-circle"></i>
                <div>
                    <h4>Успешно отправлено!</h4>
                    <p>Мы свяжемся с вами в ближайшее время</p>
                </div>
            </div>
        `;
        
        // Insert after form
        form.parentNode.insertBefore(successDiv, form.nextSibling);
        
        // Remove message after 5 seconds
        setTimeout(() => {
            successDiv.style.opacity = '0';
            successDiv.style.transform = 'translateY(-10px)';
            setTimeout(() => {
                successDiv.remove();
            }, 300);
        }, 5000);
        
        // Trigger confetti effect for brief form
        if (form.getAttribute('data-form') === 'brief') {
            this.triggerConfetti();
        }
    }
    
    showError(form, message) {
        // Create error message
        const errorDiv = document.createElement('div');
        errorDiv.className = 'form-error glass-card';
        errorDiv.innerHTML = `
            <div class="error-content">
                <i class="fas fa-exclamation-circle"></i>
                <div>
                    <h4>Ошибка отправки</h4>
                    <p>${message}</p>
                </div>
            </div>
        `;
        
        // Insert after form
        form.parentNode.insertBefore(errorDiv, form.nextSibling);
        
        // Remove message after 5 seconds
        setTimeout(() => {
            errorDiv.remove();
        }, 5000);
    }
    
    triggerConfetti() {
        // Simple confetti effect
        if (typeof confetti === 'function') {
            confetti({
                particleCount: 100,
                spread: 70,
                origin: { y: 0.6 }
            });
        }
    }
    
    trackFormSubmission(formType, formData) {
        // Track in Google Analytics (if available)
        if (typeof gtag === 'function') {
            gtag('event', 'form_submission', {
                'event_category': 'form',
                'event_label': formType
            });
        }
        
        // Log to console for debugging
        console.log(`Form submitted: ${formType}`, Object.fromEntries(formData));
    }
    
    // Validate email
    validateEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
    
    // Validate phone
    validatePhone(phone) {
        const re = /^[\+]?[0-9\s\-\(\)]+$/;
        return re.test(phone);
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new FormHandler();
    
    // Add real-time validation
    const emailInputs = document.querySelectorAll('input[type="email"]');
    emailInputs.forEach(input => {
        input.addEventListener('blur', function() {
            if (this.value && !this.validateEmail(this.value)) {
                this.classList.add('invalid');
                this.nextElementSibling?.classList.add('show');
            } else {
                this.classList.remove('invalid');
                this.nextElementSibling?.classList.remove('show');
            }
        });
    });
});

// Add to HTMLElement prototype for convenience
HTMLElement.prototype.validateEmail = function() {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(this.value);
};