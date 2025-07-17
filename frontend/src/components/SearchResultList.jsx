import React, { useEffect, useState } from 'react'

function SearchResultList() {
  const [items, setItems] = useState([])
  const [query, setQuery] = useState('')

  useEffect(() => {
    fetch('/search/items/')
      .then(res => res.json())
      .then(setItems)
      .catch(() => setItems([]))
  }, [])

  const filtered = items.filter(i =>
    i.description.toLowerCase().includes(query.toLowerCase())
  )

  return (
    <div>
      <input
        placeholder="Search"
        value={query}
        onChange={e => setQuery(e.target.value)}
      />
      <table>
        <thead>
          <tr>
            <th>Select</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {filtered.map(item => (
            <tr key={item.id}>
              <td><input type="checkbox" /></td>
              <td>{item.description}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default SearchResultList
