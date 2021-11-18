
import React, { useState, useEffect } from 'react';
import logo from './logo.png';
import { FaBars } from 'react-icons/fa';
import { IconContext } from 'react-icons/lib';
import { animateScroll as scroll } from 'react-scroll';
import {
  MobileIcon,
  Nav,
  NavbarContainer,
  NavItem,
  NavLinks,
  NavLogo,
  NavMenu,
  NavBtn,
  NavBtnLink
} from './NavbarElements';

const Navbar = ({ toggle }) => {
  const [scrollNav, setScrollNav] = useState(false);
  const [loggedIn, setLoggedIn] = useState(false);

  const changeNav = () => {
    if (window.scrollY >= 80) {
      setScrollNav(true);
    } else {
      setScrollNav(false);
    }
  };

  useEffect(() => {
    window.addEventListener('scroll', changeNav);
    isLoggedIn();
  }, []);

  const toggleHome = () => {
    scroll.scrollToTop();
  };

  const isLoggedIn = () => {
    const localUserString = window.localStorage.getItem("user");
    if (localUserString === null || localUserString === 'undefined') {
      setLoggedIn(false); return;
    }  
    const parsedUser = JSON.parse(localUserString);
    if (parsedUser["tokenExpires"] <= Date.now()) {
      setLoggedIn(false); return;
    }
    setLoggedIn(true);
  };

  const userBtn = () => {
    if (loggedIn) {
      return (
        <NavBtnLink to='/signout'>Sign Out</NavBtnLink>
      );
    }
    else {
      return (
        <NavBtnLink to='/signin'>Sign In</NavBtnLink>
      );
    }
  };

  return (
    <>
      <IconContext.Provider value={{ color: '#fff' }}>
        <Nav scrollNav={scrollNav}>
          <NavbarContainer>
            <NavLogo onClick={toggleHome} to='/'>
              <img style={{heigh: 50, width: 60}} src={logo} alt='Logo' />
            </NavLogo>
            <MobileIcon onClick={toggle}>
              <FaBars />
            </MobileIcon>
            <NavMenu>
              <NavItem>
                <NavLinks
                  to='about'
                  smooth={true}
                  duration={500}
                  spy={true}
                  exact='true'
                  offset={-80}
                >
                  About
                </NavLinks>
              </NavItem>
              <NavItem>
                <NavLinks
                  to='discover'
                  smooth={true}
                  duration={500}
                  spy={true}
                  exact='true'
                  offset={-80}
                >
                  Discover
                </NavLinks>
              </NavItem>
              <NavItem>
                <NavLinks
                  to='services'
                  smooth={true}
                  duration={500}
                  spy={true}
                  exact='true'
                  offset={-80}
                >
                  Services
                </NavLinks>
              </NavItem>
              <NavItem>
                <NavLinks
                  to='signup'
                  smooth={true}
                  duration={500}
                  spy={true}
                  exact='true'
                  offset={-80}
                >
                  Sign Up
                </NavLinks>
              </NavItem>
            </NavMenu>
            <NavBtn>{userBtn()}</NavBtn>
          </NavbarContainer>
        </Nav>
      </IconContext.Provider>
    </>
  );
};

export default Navbar;
