import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { CheckCircle } from 'lucide-react';
import { useCart } from '../../context/CartContext';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';

const Checkout = () => {
    const { cartTotal, clearCart } = useCart();
    const navigate = useNavigate();
    const [step, setStep] = useState(1); // 1: Info, 2: Payment, 3: Success
    const [isLoading, setIsLoading] = useState(false);

    const handlePayment = async (e) => {
        e.preventDefault();
        setIsLoading(true);
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 2000));
        setIsLoading(false);
        setStep(3);
        clearCart();
    };

    if (step === 3) {
        return (
            <div className="min-h-[60vh] flex flex-col items-center justify-center p-4 text-center">
                <div className="w-20 h-20 bg-green-100 text-green-600 rounded-full flex items-center justify-center mb-6">
                    <CheckCircle className="w-10 h-10" />
                </div>
                <h2 className="text-3xl font-heading font-bold text-primary mb-2">Order Confirmed!</h2>
                <p className="text-textSecondary mb-8 max-w-md">
                    Thank you for your purchase. A confirmation email has been sent to you.
                    The artist has been notified to prepare your shipment.
                </p>
                <Link to="/marketplace">
                    <Button variant="primary">Continue Shopping</Button>
                </Link>
            </div>
        );
    }

    return (
        <div className="max-w-4xl mx-auto px-4 py-12">
            <h1 className="text-3xl font-heading font-bold text-primary mb-8 px-4">Checkout</h1>

            <div className="bg-white rounded-2xl shadow-lg border border-border overflow-hidden">
                {/* Progress Steps */}
                <div className="flex border-b border-border bg-gray-50">
                    <div className={`flex-1 py-4 text-center text-sm font-bold ${step >= 1 ? 'text-secondary border-b-2 border-secondary' : 'text-gray-400'}`}>1. Shipping Info</div>
                    <div className={`flex-1 py-4 text-center text-sm font-bold ${step >= 2 ? 'text-secondary border-b-2 border-secondary' : 'text-gray-400'}`}>2. Payment</div>
                    <div className={`flex-1 py-4 text-center text-sm font-bold ${step >= 3 ? 'text-secondary border-b-2 border-secondary' : 'text-gray-400'}`}>3. Confirmation</div>
                </div>

                <div className="p-8">
                    {step === 1 && (
                        <form onSubmit={(e) => { e.preventDefault(); setStep(2); }} className="space-y-6">
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <Input label="First Name" required />
                                <Input label="Last Name" required />
                            </div>
                            <Input label="Address" required />
                            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                                <Input label="City" required />
                                <Input label="State/Province" required />
                                <Input label="Zip Code" required />
                            </div>
                            <Input label="Phone Number" required />

                            <div className="flex justify-end pt-4">
                                <Button type="submit" variant="primary">Continue to Payment</Button>
                            </div>
                        </form>
                    )}

                    {step === 2 && (
                        <form onSubmit={handlePayment} className="space-y-6">
                            <div className="p-4 bg-gray-50 rounded-lg border border-border mb-6">
                                <div className="flex justify-between mb-2">
                                    <span className="text-gray-600">Total Amount</span>
                                    <span className="font-bold text-lg">${cartTotal + 50}</span>
                                </div>
                                <p className="text-xs text-gray-400">Including shipping</p>
                            </div>

                            <Input label="Card Number" placeholder="0000 0000 0000 0000" required />
                            <div className="grid grid-cols-2 gap-6">
                                <Input label="Expiry Date" placeholder="MM/YY" required />
                                <Input label="CVC" placeholder="123" required />
                            </div>

                            <div className="flex justify-between items-center pt-4">
                                <utton type="button" variant="ghost" onClick={() => setStep(1)}>Back</utton>
                                <Button type="submit" variant="primary" isLoading={isLoading}>Pay Now</Button>
                            </div>
                        </form>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Checkout;
