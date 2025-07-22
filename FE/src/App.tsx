import { BrowserRouter as Router } from 'react-router-dom';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { initializeAuth } from './store/slices/authSlice';
import RoutesComponent from './routes';
import { RootState } from './store';
import cookieUtils from './services/cookieUtils';

const App = () => {
  const dispatch = useDispatch();
  const { loading } = useSelector((state: RootState) => state.auth);
  const token = cookieUtils.getToken();

  useEffect(() => {
    dispatch(initializeAuth());
  }, [dispatch]);

  if (loading && token) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <Router>
        <RoutesComponent />
      </Router>
    </>
  );
};

export default App;
