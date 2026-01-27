import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Loader from '../components/common/Loader';

const ProtectedRoute = ({ children }) => {
    const { user, loading } = useAuth();

    if (loading) {
        return <Loader fullScreen />;
    }

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    // Could add role check here later
    return children;
};

export default ProtectedRoute;
