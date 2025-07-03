import { useSelector } from 'react-redux';
import { RootState } from '@/store';
import Navbar from "./Navbar/Navbar";
import TopMenu from "./TopMenu/TopMenu";

const Headers = () => {
    const { user } = useSelector((state: RootState) => state.auth);
    const role = user?.role || null;
    
    return (
        <>
            <TopMenu
                role={role}
            />
            <Navbar/>
        </>
    );
}

export default Headers