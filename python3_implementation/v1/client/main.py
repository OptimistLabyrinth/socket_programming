import socket

terminate_string = '-term-end-'
BUFF_SIZE = 10

if __name__ == "__main__":
    while True:
        string = input('Enter message to server: ')

        if len(string) == 0:
            continue

        if string == 'exit' or string == 'quit':
            break

        string += terminate_string

        conn_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        conn_socket.connect(('127.0.0.1', 8080))
        send_len = conn_socket.send(string.encode('utf-8'))

        if send_len != len(string):
            print('error on send')

        result = ''

        while True:
            data = conn_socket.recv(BUFF_SIZE).decode('utf-8')

            print(f'    {data}')
            if data.find(terminate_string) != -1:
                data = data.replace(terminate_string, '')
                result += data
                break

            result += data
            if len(data) < BUFF_SIZE:
                break

        if result.find(terminate_string) != -1:
            result = result.replace(terminate_string, '')
        print(f'result: {result}')
