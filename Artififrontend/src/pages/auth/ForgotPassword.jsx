import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import { Mail, ArrowLeft } from 'lucide-react';

const ForgotPassword = () => {
    // Basic placeholder implementation since backend is not ready
    return (
        <div className="min-h-[80vh] flex items-center justify-center px-4">
            <div className="w-full max-w-md bg-white p-8 rounded-2xl shadow-xl border border-border">
                <div className="mb-6">
                    <Link to="/login" className="flex items-center text-sm text-textSecondary hover:text-primary mb-4">
                        <ArrowLeft className="w-4 h-4 mr-1" /> Back to Login
                    </Link>
                    <h2 className="text-2xl font-bold text-primary mb-2">Forgot Password?</h2>
                    <p className="text-textSecondary">Enter your email and we'll send you instructions to reset your password.</p>
                </div>

                <form className="space-y-6">
                    <Input label="Email Address" type="email" placeholder="you@example.com" icon={Mail} required />
                    <Button variant="primary" className="w-full">Send Reset Link</Button>
                </form>
            </div>
        </div>
    );
};

export default ForgotPassword;
