import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../../services/auth';
import './Register.css';

function Register() {
  const [userData, setUserData] = useState({
    email: '',
    username: '',
    password: '',
  });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleRegister = async () => {
    try {
      await register(userData);
      navigate('/');
      window.location.reload();
    } catch (error) {
      setError(error.response?.data?.message || 'Registration failed. Please try again.');
    }
  };

  return (
    <div className="register-container">
      <div className="register-box">
        <h1 className="register-heading">Register</h1>
        {error && <p className="error-text">{error}</p>}
        <div className="register-form">
          <input
            className="register-input"
            placeholder="Email"
            name="email"
            type="email"
            value={userData.email}
            onChange={handleChange}
            maxLength={254}
          />
          <input
            className="register-input"
            placeholder="Username"
            name="username"
            value={userData.username}
            onChange={handleChange}
          />
          <input
            className="register-input"
            placeholder="Password"
            name="password"
            type="password"
            maxLength={20}
            value={userData.password}
            onChange={handleChange}
          />
          <button className="register-button" onClick={handleRegister}>
            Register
          </button>
        </div>
      </div>
    </div>
  );
}

export default Register;