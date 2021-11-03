import React from 'react';
import TransactionHistory from '../../components/TransactionHistory';
import ScrollToTop from '../../components/ScrollToTop';

function TransactionHistoryPage() {
  return (
    <>
      <ScrollToTop />
      <TransactionHistory />
    </>
  );
}

export default TransactionHistoryPage;