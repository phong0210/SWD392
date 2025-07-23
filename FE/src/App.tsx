import { BrowserRouter as Router } from 'react-router-dom';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { AppDispatch } from './store';
import { initializeAuth } from './store/slices/authSlice';
import RoutesComponent from './routes';
// import { RootState } from './store';
// import cookieUtils from './services/cookieUtils';

const App = () => {
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    dispatch(initializeAuth());
  }, [dispatch]);

  return (
    <>
      <Router>
        <RoutesComponent />
      </Router>
    </>
  );
};

export default App;
