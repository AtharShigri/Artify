import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import SEO from '../../components/common/SEO';
import { ART_CATEGORIES } from '../../constants/categories';
import { Building2, User, ChevronRight, ChevronLeft, CheckCircle2 } from 'lucide-react';

const Register = () => {
    const [step, setStep] = useState(1);
    const [formData, setFormData] = useState({
        fullName: '',
        email: '',
        password: '',
        confirmPassword: '',
        category: '',
        role: 'buyer',      // 'buyer' | 'artist'
        userType: 0,        // 0 = Individual, 1 = Agency
        teamSize: '',
        memberNames: ''
    });
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const { register } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleNext = () => {
        setError('');
        if (step === 1) {
            setStep(2);
        } else if (step === 2) {
            // Validation for step 2
            if (formData.userType === 1 && !formData.teamSize) {
                setError('Please select/enter your agency size.');
                return;
            }
            setStep(3);
        }
    };

    const handleBack = () => {
        setStep(step - 1);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        setIsLoading(true);

        try {
            const { confirmPassword, ...payload } = formData;
            // Map teamSize to integer for API
            payload.userType = parseInt(payload.userType);
            payload.teamSize = payload.teamSize ? parseInt(payload.teamSize) : null;
            
            await register(payload);
            navigate('/login');
        } catch (err) {
            setError(err.message || 'Failed to register');
        } finally {
            setIsLoading(false);
        }
    };

    const toggleClass = (active) =>
        `flex flex-col items-center justify-center gap-3 p-6 text-center rounded-2xl border-2 transition-all cursor-pointer h-full ${
            active
                ? 'bg-secondary/5 border-secondary text-secondary shadow-lg shadow-secondary/10'
                : 'bg-white border-gray-100 text-textSecondary hover:border-gray-200 hover:bg-gray-50'
        }`;

    const renderStep1 = () => (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
            <div className="text-center">
                <h3 className="text-xl font-bold text-primary mb-2">First, tell us about yourself</h3>
                <p className="text-textSecondary text-sm">How do you plan to use Artify?</p>
            </div>
            <div className="grid grid-cols-2 gap-4">
                <div 
                    onClick={() => setFormData({ ...formData, userType: 0 })}
                    className={toggleClass(formData.userType === 0)}
                >
                    <div className={`w-12 h-12 rounded-full flex items-center justify-center ${formData.userType === 0 ? 'bg-secondary text-white' : 'bg-gray-100 text-gray-500'}`}>
                        <User className="w-6 h-6" />
                    </div>
                    <div>
                        <p className="font-bold">Individual</p>
                        <p className="text-xs mt-1 text-textSecondary opacity-80">Personal account for artists & buyers</p>
                    </div>
                </div>
                <div 
                    onClick={() => setFormData({ ...formData, userType: 1 })}
                    className={toggleClass(formData.userType === 1)}
                >
                    <div className={`w-12 h-12 rounded-full flex items-center justify-center ${formData.userType === 1 ? 'bg-secondary text-white' : 'bg-gray-100 text-gray-500'}`}>
                        <Building2 className="w-6 h-6" />
                    </div>
                    <div>
                        <p className="font-bold">Agency</p>
                        <p className="text-xs mt-1 text-textSecondary opacity-80">Teams, studios, and art businesses</p>
                    </div>
                </div>
            </div>
            <Button onClick={handleNext} className="w-full h-12 rounded-xl text-lg font-bold">
                Continue <ChevronRight className="ml-2 w-5 h-5" />
            </Button>
        </div>
    );

    const renderStep2 = () => {
        if (formData.userType === 1) {
            // Agency Specific Step
            return (
                <div className="space-y-6 animate-in fade-in slide-in-from-right-4 duration-500">
                    <div className="text-center">
                        <h3 className="text-xl font-bold text-primary mb-2">Agency Details</h3>
                        <p className="text-textSecondary text-sm">Tell us more about your team</p>
                    </div>

                    <div className="space-y-4">
                        <label className="block text-sm font-semibold text-primary">Size of Agency</label>
                        <div className="grid grid-cols-2 gap-3">
                            {['1-2', '3-6', '7-20', '20+'].map((size) => (
                                <button
                                    key={size}
                                    type="button"
                                    onClick={() => setFormData({ ...formData, teamSize: size.includes('-') ? size.split('-')[1] : (size.includes('+') ? 50 : size) })}
                                    className={`py-3 px-4 rounded-xl border-2 transition-all font-medium ${
                                        (formData.teamSize === (size.includes('-') ? size.split('-')[1] : (size.includes('+') ? 50 : size)) )
                                            ? 'border-secondary bg-secondary/5 text-secondary'
                                            : 'border-gray-100 hover:border-gray-200'
                                    }`}
                                >
                                    {size} members
                                </button>
                            ))}
                        </div>

                        {formData.teamSize && parseInt(formData.teamSize) < 7 && (
                            <div className="pt-2 animate-in fade-in duration-300">
                                <label className="block text-sm font-semibold text-primary mb-2">Member Names</label>
                                <textarea
                                    name="memberNames"
                                    value={formData.memberNames}
                                    onChange={handleChange}
                                    placeholder="Enter colleague names (comma separated)"
                                    className="w-full px-4 py-3 rounded-xl border border-border focus:ring-2 focus:ring-secondary/20 focus:border-secondary outline-none transition-all min-h-[100px] bg-gray-50/50"
                                />
                            </div>
                        )}
                    </div>

                    <div className="flex gap-3 pt-4">
                        <Button variant="ghost" onClick={handleBack} className="w-1/3">
                            <ChevronLeft className="mr-1 w-4 h-4" /> Back
                        </Button>
                        <Button onClick={handleNext} className="w-2/3">
                            Next Step <ChevronRight className="ml-1 w-4 h-4" />
                        </Button>
                    </div>
                </div>
            );
        }

        // Individual Specific Step
        return (
            <div className="space-y-6 animate-in fade-in slide-in-from-right-4 duration-500">
                <div className="text-center">
                    <h3 className="text-xl font-bold text-primary mb-2">Who are you?</h3>
                    <p className="text-textSecondary text-sm">Choose the role that fits you best</p>
                </div>
                <div className="grid grid-cols-1 gap-4">
                    <div 
                        onClick={() => setFormData({ ...formData, role: 'artist' })}
                        className={`p-4 rounded-2xl border-2 cursor-pointer transition-all flex items-center gap-4 ${
                            formData.role === 'artist' ? 'border-secondary bg-secondary/5' : 'border-gray-100'
                        }`}
                    >
                        <div className={`w-10 h-10 rounded-full flex items-center justify-center shrink-0 ${formData.role === 'artist' ? 'bg-secondary text-white' : 'bg-gray-100 text-gray-500'}`}>
                            <CheckCircle2 className="w-5 h-5" />
                        </div>
                        <div className="text-left">
                            <p className="font-bold">I'm an Artist</p>
                            <p className="text-xs text-textSecondary text-balance">I want to showcase and sell my creative masterpieces.</p>
                        </div>
                    </div>
                    <div 
                        onClick={() => setFormData({ ...formData, role: 'buyer', category: '' })}
                        className={`p-4 rounded-2xl border-2 cursor-pointer transition-all flex items-center gap-4 ${
                            formData.role === 'buyer' ? 'border-secondary bg-secondary/5' : 'border-gray-100'
                        }`}
                    >
                        <div className={`w-10 h-10 rounded-full flex items-center justify-center shrink-0 ${formData.role === 'buyer' ? 'bg-secondary text-white' : 'bg-gray-100 text-gray-500'}`}>
                            <CheckCircle2 className="w-5 h-5" />
                        </div>
                        <div className="text-left">
                            <p className="font-bold">I'm an Art Enthusiast</p>
                            <p className="text-xs text-textSecondary text-balance">I want to discover and purchase unique artworks.</p>
                        </div>
                    </div>
                </div>
                <div className="flex gap-3 pt-4">
                    <Button variant="ghost" onClick={handleBack} className="w-1/3 text-textSecondary">
                        Back
                    </Button>
                    <Button onClick={handleNext} className="w-2/3">
                        Continue <ChevronRight className="ml-2 w-5 h-5" />
                    </Button>
                </div>
            </div>
        );
    };

    const renderStep3 = () => (
        <form onSubmit={handleSubmit} className="space-y-5 animate-in fade-in slide-in-from-right-4 duration-500">
            <div className="text-center mb-2">
                <h3 className="text-xl font-bold text-primary mb-1">Create Account</h3>
                <p className="text-textSecondary text-xs">Almost there! Fill in your credentials</p>
            </div>

            <Input
                label="Full Name"
                name="fullName"
                value={formData.fullName}
                onChange={handleChange}
                placeholder="Ex: John Doe"
                required
            />

            <Input
                label="Email Address"
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="john@example.com"
                required
            />

            {(formData.role === 'artist' || formData.userType === 1) && (
                <div>
                    <label className="block text-sm font-medium text-textSecondary mb-1.5">Art Category</label>
                    <select
                        name="category"
                        value={formData.category}
                        onChange={handleChange}
                        required
                        className="w-full px-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all bg-white"
                    >
                        <option value="">Select a Category</option>
                        {ART_CATEGORIES.map((cat) => (
                            <option key={cat} value={cat}>{cat}</option>
                        ))}
                    </select>
                </div>
            )}

            <div className="grid grid-cols-2 gap-3">
                <Input
                    label="Password"
                    type="password"
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    required
                />
                <Input
                    label="Confirm"
                    type="password"
                    name="confirmPassword"
                    value={formData.confirmPassword}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="flex gap-3 pt-2">
                <Button variant="ghost" type="button" onClick={handleBack} className="w-1/3 text-textSecondary">
                    Back
                </Button>
                <Button type="submit" variant="primary" className="w-2/3" isLoading={isLoading}>
                    Complete
                </Button>
            </div>
        </form>
    );

    return (
        <div className="min-h-[90vh] flex items-center justify-center px-4 py-20 bg-gray-50/30">
            <SEO title="Register" description="Join Artify as an agency, artist or buyer." />
            <div className="w-full max-w-lg bg-white p-10 rounded-[2.5rem] shadow-2xl shadow-primary/5 border border-gray-100">
                <div className="text-center mb-10">
                    <Link to="/" className="inline-block mb-2">
                        <span className="font-heading font-black text-4xl text-primary tracking-tight">Artify<span className="text-secondary">.</span></span>
                    </Link>
                    
                    {/* Progress Bar */}
                    <div className="flex items-center justify-center gap-2 mt-4">
                        {[1, 2, 3].map(s => (
                            <div 
                                key={s} 
                                className={`h-1.5 rounded-full transition-all duration-300 ${s === step ? 'w-8 bg-secondary' : 'w-4 bg-gray-100'}`} 
                            />
                        ))}
                    </div>
                </div>

                {error && (
                    <div className="mb-6 p-4 bg-red-50 text-error text-sm rounded-xl border border-red-100 animate-in shake duration-300">
                        {error}
                    </div>
                )}

                {step === 1 && renderStep1()}
                {step === 2 && renderStep2()}
                {step === 3 && renderStep3()}

                <p className="mt-10 text-center text-sm text-textSecondary font-medium">
                    Already have an account?{' '}
                    <Link to="/login" className="text-secondary hover:text-primary transition-colors hover:underline">
                        Sign in
                    </Link>
                </p>
            </div>
        </div>
    );
};

export default Register;

