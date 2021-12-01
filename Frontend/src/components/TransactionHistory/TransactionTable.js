import React, { useMemo } from 'react';
import styled from 'styled-components';
import {
    useTable
} from 'react-table';

const TransactionTable = (props) =>  {
    const data = useMemo(() => props.data, [props.data]);
    const columns = useMemo(() => props.columns, [props.columns]);
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        prepareRow,
        rows,
    } = useTable({ columns, data, });
    return (
        <div>
            <table {...getTableProps()} 
            style={{
                borderBottom: 'solid 3px blue',
                background: 'black',
                color: 'white',
                fontWeight: 'bold',
                border: '1px solid black',
              }}
            >
                <thead>
                    {headerGroups.map(headerGroup => (
                        <tr {...headerGroup.getHeaderGroupProps()}
                        style={{
                            borderBottom: 'solid 3px blue',
                            background: 'green',
                            color: 'white',
                            fontWeight: 'normal',
                            border: '1px solid black',
                          }}
                        
                        >
                            {headerGroup.headers.map(column => (
                                <th {...column.getHeaderProps()}>
                                    {column.render('Header')}
                                </th>
                            ))}
                        </tr>
                    ))}
                </thead>
                <tbody {...getTableBodyProps()} className="TransactionsTableBody">
                    {rows.map(row => {
                        prepareRow(row)
                        return (
                            <tr {...row.getRowProps()}
                            
                            style={{
                                borderBottom: 'solid 3px blue',
                                background: 'white',
                                color: 'black',
                                fontWeight: 'normal',
                                border: '1px solid black',
                              }}
                            >
                                {row.cells.map(cell => {
                                    return (
                                        <td {...cell.getCellProps()}>
                                            {cell.render("Cell")}
                                        </td>
                                    )
                                })}
                            </tr>
                        )
                    })}
                </tbody>
            </table>
        </div>
    )
}

export default TransactionTable;