import React, { useState } from 'react';
import Navbar from '../../components/Navbar';
import Sidebar from '../../components/Sidebar';
import Footer from '../../components/Footer';
import AccountSidebar from '../../components/AccountSidebar';
import MessageSection from '../../components/MessageSection';

function DashboardPage() {
  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => {
    setIsOpen(!isOpen);
  };
  return (
    <>
      <Sidebar isOpen={isOpen} toggle={toggle} />
      <Navbar toggle={toggle} />
      <MessageSection/>
      <AccountSidebar />
      <Footer />
    </>
  );
}

export default DashboardPage;