import React, { useState } from 'react';
import { ShieldCheck, ArrowRight, Loader2 } from 'lucide-react';
import { paymentService } from '../../services/paymentService';

const EscrowActionCard = ({ role, orderId, defaultAmount = 0, status, onActionSuccess }) => {
    const [amount, setAmount] = useState(defaultAmount);
    const [loading, setLoading] = useState(false);

    const handleInitiateEscrow = async () => {
        try {
            setLoading(true);
            await paymentService.initiateEscrow(orderId, amount);
            onActionSuccess?.();
            setLoading(false);
        } catch (error) {
            console.error(error);
            setLoading(false);
            alert("Failed to initiate payment");
        }
    };

    const handleConfirmReceipt = async () => {
        try {
            setLoading(true);
            await paymentService.confirmReceipt(orderId);
            onActionSuccess?.();
            setLoading(false);
        } catch (error) {
            console.error(error);
            setLoading(false);
            alert("Failed to confirm receipt");
        }
    };

    return (
        <div className="my-4 p-4 border border-blue-200 bg-blue-50 rounded-xl max-w-sm mx-auto shadow-sm">
            <div className="flex items-center gap-2 mb-3 text-blue-800">
                <ShieldCheck size={20} />
                <h4 className="font-semibold text-sm">Secure Escrow Payment</h4>
            </div>

            {role === 'Artist' && status === 'Pending' && (
                <div className="flex flex-col gap-3">
                    <p className="text-xs text-blue-700">Create an invoice/offer to request payment for your service.</p>
                    <div className="flex items-center gap-2">
                        <span className="font-medium">$</span>
                        <input
                            type="number"
                            className="bg-white border text-sm w-full p-2 rounded-lg"
                            value={amount}
                            onChange={(e) => setAmount(Number(e.target.value))}
                            placeholder="Enter amount"
                        />
                    </div>
                    <button 
                        className="bg-primary text-white py-2 px-4 rounded-lg text-sm font-medium hover:bg-primary/90 transition"
                        onClick={() => {
                            // Normally an artist would create the invoice, meaning the order update 
                            // would happen here. For now it's just mock visual or custom handler
                            onActionSuccess?.(amount);
                        }}
                    >
                        Create Invoice/Offer
                    </button>
                </div>
            )}

            {role === 'Buyer' && status === 'Pending' && (
                <div className="flex flex-col gap-3">
                    <p className="text-xs text-blue-700">Artist has requested <strong>${amount}</strong> for this project.</p>
                    <button 
                        onClick={handleInitiateEscrow}
                        disabled={loading}
                        className="flex justify-center items-center gap-2 bg-green-600 text-white py-2 px-4 rounded-lg text-sm font-medium hover:bg-green-700 transition disabled:opacity-50"
                    >
                        {loading ? <Loader2 size={16} className="animate-spin" /> : "Accept & Pay (Escrow)"}
                    </button>
                    <p className="text-[10px] text-blue-600 text-center">Funds are held securely until the service is done.</p>
                </div>
            )}

            {role === 'Buyer' && status === 'Held' && (
                <div className="flex flex-col gap-3">
                    <p className="text-xs text-blue-700">Funds are currently in escrow. Release them once the service is completed to your satisfaction.</p>
                    <button 
                        onClick={handleConfirmReceipt}
                        disabled={loading}
                        className="flex justify-center items-center gap-2 bg-blue-600 text-white py-2 px-4 rounded-lg text-sm font-medium hover:bg-blue-700 transition disabled:opacity-50"
                    >
                        {loading ? <Loader2 size={16} className="animate-spin" /> : "Mark as Received"}
                    </button>
                </div>
            )}

            {role === 'Artist' && status === 'Held' && (
                <div className="flex flex-col gap-3">
                    <p className="text-xs text-blue-700">Funds are securely held in escrow. The buyer will release them upon project completion.</p>
                </div>
            )}

            {status === 'Released' && (
                <div className="flex flex-col gap-2 items-center justify-center p-2 text-green-700">
                    <ShieldCheck size={24} className="text-green-600" />
                    <p className="text-sm font-semibold">Payment Released</p>
                </div>
            )}
        </div>
    );
};

export default EscrowActionCard;
