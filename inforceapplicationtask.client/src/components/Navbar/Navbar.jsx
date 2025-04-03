import React, { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { getCurrentToken, getCurrentUser, getCurrentUsername, refreshToken, logout } from '../../services/auth';
import './Navbar.css';

function Navbar() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  const checkAuthStatus = () => {
    const token = getCurrentToken();
    const currentUser = getCurrentUser();
    if (token && currentUser) {
      setIsAuthenticated(true);
      setUser(currentUser);
    } else {
      setIsAuthenticated(false);
      setUser(null);
    }
  };

  useEffect(() => {
    checkAuthStatus();
  }, []);

  useEffect(() => {
    const interval = setInterval(() => {
      if (isAuthenticated) {
        refreshToken();
      }
    }, 10 * 60 * 1000);
    return () => clearInterval(interval);
  }, [isAuthenticated]);

  const handleLogout = async () => {
    try {
      await logout();
      setIsAuthenticated(false);
      setUser(null);
      navigate('/');
      window.location.reload();
    } catch (error) {
      console.error('Logout failed', error);
    }
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-brand">
          <Link to="/">Url Shortener</Link>
        </div>
        <div className="navbar-links">
          <Link to="/">Home</Link>
          <Link to="/urls">Urls</Link>
          <Link to="/about">About</Link>
          {isAuthenticated ? (
            <>
              <span className="navbar-user">Welcome, {getCurrentUsername()}</span>
              <button className="navbar-button" onClick={handleLogout}>Logout</button>
            </>
          ) : (
            <>
              <Link to="/login">Login</Link>
              <Link to="/register">Register</Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
